using AutoMapper;
using Azure;
using Microservices.Services.CartAPI.Data;
using Microservices.Services.CartAPI.Models;
using Microservices.Services.CartAPI.Models.Dto;
using Microservices.Services.CartAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Microservices.Services.CartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IProductService productService;
        private ResponseDto _response;

        public CartApiController(AppDbContext appDbContext, IMapper mapper, IProductService productService)
        {
            this._db = appDbContext;
            this._mapper = mapper;
            this.productService = productService;
            this._response = new ResponseDto();
        }


        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = new CartDto()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_db.CartHeaders.First(x => x.UserId == userId))
                };
                cartDto.cartDetails = _mapper.Map<List<CartDetailsDto>>(_db.CartDetails.Where(x => x.CartHeaderId == cartDto.CartHeader.CartHeaderId));

                IEnumerable<ProductDto> productDto = await productService.Getproducts();

                foreach (var item in cartDto.cartDetails)
                {
                    item.Product = productDto.FirstOrDefault(x => x.ProductId == item.ProductId);
                    cartDto.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {

                    //Create header and Details
                    CartHeaders cartHeaders = _mapper.Map<CartHeaders>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeaders);
                    await _db.SaveChangesAsync();
                    cartDto.cartDetails.First().CartHeaderId = cartHeaders.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.cartDetails.First()));
                    await _db.SaveChangesAsync();

                }
                else
                {
                    //if header is not null 
                    //check if details has same product.

                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        x => x.ProductId == cartDto.cartDetails.FirstOrDefault().ProductId &&
                        x.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {

                        //create cart details
                        cartDto.cartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.cartDetails.First()));
                        await _db.SaveChangesAsync();

                    }
                    else
                    {

                        //Update Count in cart details
                        cartDto.cartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.cartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        cartDto.cartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.cartDetails.First()));
                        await _db.SaveChangesAsync();

                    }
                }
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _db.CartDetails.First(x => x.CartDetailsId == cartDetailsId);
                int totalCartItmes = _db.CartDetails.Where(x => x.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails);
                if (totalCartItmes == 1)
                {
                    var cartHeaderRemove = await _db.CartHeaders.FirstOrDefaultAsync(x => x.CartHeaderId == cartDetails.CartHeaderId);
                    _db.CartHeaders.Remove(cartHeaderRemove);
                }
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
