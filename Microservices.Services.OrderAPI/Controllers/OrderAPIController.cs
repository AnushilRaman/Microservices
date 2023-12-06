using AutoMapper;
using Microservices.MessageBus;
using Microservices.Services.OrderAPI.Data;
using Microservices.Services.OrderAPI.Models;
using Microservices.Services.OrderAPI.Models.Dto;
using Microservices.Services.OrderAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        protected ResponseDto _responseDto;
        private IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        private IProductService _productService;
        private readonly IMessageBus messageBus;

        public OrderApiController(IMapper _mapper, AppDbContext _appDbContext, IProductService _productService, IMessageBus messageBus)
        {
            this._mapper = _mapper;
            this._appDbContext = _appDbContext;
            this._productService = _productService;
            this.messageBus = messageBus;
            this._responseDto = new ResponseDto();
        }

        [Authorize]
        [HttpGet("GetOrders")]
        public ResponseDto? GetOrders(string? userid = "")
        {
            try
            {
                IEnumerable<OrderHeader> objList;
                if (User.IsInRole(StaticClass.RoleAdmin))
                {
                    objList = _appDbContext.OrderHeaders.Include(x => x.orderDetails).OrderByDescending(x => x.OrderHeaderId).ToList();
                }
                else
                {
                    objList = _appDbContext.OrderHeaders.Include(x => x.orderDetails).Where(x => x.UserId == userid).OrderByDescending(x => x.OrderHeaderId).ToList();
                }
                _responseDto.Result = _mapper.Map<List<OrderHeaderDto>>(objList);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }


        [Authorize]
        [HttpGet("GetOrder/{id:int}")]
        public ResponseDto? GetOrder(int id)
        {
            try
            {
                OrderHeader orderHeader = _appDbContext.OrderHeaders.Include(x => x.OrderHeaderId).First(x => x.OrderHeaderId == id);
                _responseDto.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }


        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody] CartDto cartDto)
        {
            try
            {
                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(cartDto.CartHeader);
                orderHeaderDto.OrderTime = DateTime.Now;
                orderHeaderDto.Status = StaticClass.Status_Pending;
                orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(cartDto.cartDetails);

                OrderHeader orderCreated = _appDbContext.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
                await _appDbContext.SaveChangesAsync();

                orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;
                _responseDto.Result = orderHeaderDto;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<ResponseDto> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            try
            {

                var options = new SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDto.ApprovedUrl,
                    CancelUrl = stripeRequestDto.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };
                var discountAmt = new List<SessionDiscountOptions>()
                {
                    new SessionDiscountOptions()
                    {
                        Coupon = stripeRequestDto.OrderHeader.CouponCode
                    }
                };

                foreach (var item in stripeRequestDto.OrderHeader.OrderDetails)
                {
                    var sessionItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "inr",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name,
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionItem);
                }
                if (stripeRequestDto.OrderHeader.Discount > 0)
                {
                    options.Discounts = discountAmt;
                }
                var service = new SessionService();
                Session session = service.Create(options);
                stripeRequestDto.StripeSessionUrl = session.Url;

                OrderHeader orderHeader = _appDbContext.OrderHeaders.First(x => x.OrderHeaderId == stripeRequestDto.OrderHeader.OrderHeaderId);
                orderHeader.StripeSessionId = session.Id;
                await _appDbContext.SaveChangesAsync();
                _responseDto.Result = stripeRequestDto;

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<ResponseDto> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            try
            {
                OrderHeader orderHeader = _appDbContext.OrderHeaders.First(x => x.OrderHeaderId == orderHeaderId);

                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);
                var paymentIntentService = new PaymentIntentService();
                PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                    //payment successfull
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = StaticClass.Status_Approved;
                    await _appDbContext.SaveChangesAsync();
                    RewardsDto rewardsDto = new()
                    {
                        OrderId = orderHeader.OrderHeaderId,
                        RewardsActivity = Convert.ToInt32(orderHeader.CartTotal),
                        UserId = orderHeader.UserId
                    };
                    await messageBus.PublishMessage(rewardsDto, StaticClass.TopicName);
                    _responseDto.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [Authorize]
        [HttpPost("UpdateOrderStatus/{orderid:int}")]
        public async Task<ResponseDto> UpdateOrderStatus(int orderid, [FromBody] string newStatus)
        {
            try
            {
                OrderHeader orderHeader = await _appDbContext.OrderHeaders.FirstAsync(x => x.OrderHeaderId == orderid);
                if (orderHeader != null)
                {
                    if (newStatus == StaticClass.Status_Approved)
                    {
                        /// provide the refunds to the customer
                        var options = new RefundCreateOptions
                        {
                            Reason = RefundReasons.RequestedByCustomer,
                            PaymentIntent = orderHeader.PaymentIntentId,
                        };
                        var service = new RefundService();
                        Refund refund = service.Create(options);
                    }
                    orderHeader.Status = newStatus;
                    await _appDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
    }
}
