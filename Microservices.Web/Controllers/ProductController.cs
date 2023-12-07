using Microservices.Web.Models;
using Microservices.Web.Service;
using Microservices.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace Microservices.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? productList = new();
            ResponseDto response = await productService.GetAllProductAsync();
            if (response != null && response.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["errorMessage"] = response?.Message;
            }
            return View(productList);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto responseDto = await productService.CreateProductAsync(model);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["successMessage"] = "Product created successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["errorMessage"] = responseDto?.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            ResponseDto responseDto = await productService.GetProductByIdAsync(productId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
                return View(model);
            }
            else
            {
                TempData["errorMessage"] = responseDto?.Message;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto responseDto = await productService.UpdateProductsAsync(model);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["successMessage"] = "Product Updated successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["errorMessage"] = responseDto?.Message;
                }
            }
            
            return View(model);
        }
        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDto responseDto = await productService.GetProductByIdAsync(productId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
                return View(model);
            }
            else
            {
                TempData["errorMessage"] = responseDto?.Message;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto model)
        {
            ResponseDto responseDto = await productService.DeleteProductAsync(model.ProductId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["successMessage"] = "Product deleted successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["errorMessage"] = responseDto?.Message;
            }
            return View(model);
        }
    }
}
