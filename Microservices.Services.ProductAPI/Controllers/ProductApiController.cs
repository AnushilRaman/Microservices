using AutoMapper;
using Microservices.Services.ProductAPI.Data;
using Microservices.Services.ProductAPI.Models;
using Microservices.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public ProductApiController(AppDbContext appDbContext, IMapper mapper)
        {
            this._db = appDbContext;
            this._mapper = mapper;
            this._response = new ResponseDto();
        }

        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            try
            {
                IEnumerable<Product> objList = await _db.Products.ToListAsync();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ResponseDto> Get(int id)
        {
            try
            {
                Product obj = await _db.Products.FirstAsync(u => u.ProductId == id);
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("Name/{Name}")]
        public async Task<ResponseDto> GetByName(string Name)
        {
            try
            {
                Product obj = await _db.Products.FirstAsync(u => u.Name == Name);
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Post(ProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);
                await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();

                if (productDto.Image != null)
                {
                    string fileName = product.ProductId + Path.GetExtension(productDto.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        await productDto.Image.CopyToAsync(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    product.ImageUrl = "https://placehold.co/600x400";
                }
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Put([FromBody] ProductDto productDto)
        {
            try
            {
                Product obj = _mapper.Map<Product>(productDto);
                _db.Products.Update(obj);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                Product obj = _db.Products.First(u => u.ProductId == id);
                _db.Products.Remove(obj);
                await _db.SaveChangesAsync();
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
