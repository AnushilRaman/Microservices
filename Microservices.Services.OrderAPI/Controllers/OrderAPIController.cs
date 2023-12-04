using AutoMapper;
using Microservices.MessageBus;
using Microservices.Services.OrderAPI.Data;
using Microservices.Services.OrderAPI.Models;
using Microservices.Services.OrderAPI.Models.Dto;
using Microservices.Services.OrderAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public OrderApiController(IMapper _mapper, AppDbContext _appDbContext, IProductService _productService)
        {
            this._mapper = _mapper;
            this._appDbContext = _appDbContext;
            this._productService = _productService;
            this._responseDto = new ResponseDto();
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
    }
}
