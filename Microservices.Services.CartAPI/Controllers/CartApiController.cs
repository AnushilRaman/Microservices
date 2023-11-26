﻿using AutoMapper;
using Microservices.Services.CartAPI.Data;
using Microservices.Services.CartAPI.Models;
using Microservices.Services.CartAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.CartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public CartApiController(AppDbContext appDbContext, IMapper mapper)
        {
            this._db = appDbContext;
            this._mapper = mapper;
            this._response = new ResponseDto();
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.FirstOrDefaultAsync(x => x.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //Create header and Details
                    CartHeaders cartHeaders = _mapper.Map<CartHeaders>(cartDto);
                    _db.CartHeaders.Add(cartHeaders);
                    await _db.SaveChangesAsync();
                    cartDto.cartDetails.First().CartHeaderId = cartDto.CartHeader.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.cartDetails.First()));
                    await _db.SaveChangesAsync();

                }
                else
                {
                    //if header is not null 
                    //check if details has same product
                    var cartDetailsFromDb = await _db.CartDetails.FirstOrDefaultAsync(
                        x => x.ProductId == cartDto.cartDetails.FirstOrDefault().ProductId &&
                        x.CartDetailsId == cartDto.cartDetails.FirstOrDefault().CartDetailsId
                        );
                    if (cartDetailsFromDb == null)
                    {
                        //create cart details
                    }
                    else
                    {
                        //Update Count in cart details

                    }
                }
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