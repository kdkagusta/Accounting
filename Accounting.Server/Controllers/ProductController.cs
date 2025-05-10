using Accounting.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Accounting.Server.Models.DTOs.ProductDTOs;

namespace Accounting.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProducts();
                return ResponseOk(products);
            }
            catch (Exception ex)
            {
                return ResponseError(ex.Message, new List<string> { ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var products = await _productService.GetProductById(id);
                return ResponseOk(products);
            }
            catch (Exception ex)
            {
                return ResponseError(ex.Message, new List<string> { ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO productDto)
        {
            try
            {
                var products = await _productService.CreateProduct(productDto);
                return ResponseOk(products);
            }
            catch (Exception ex)
            {
                return ResponseError(ex.Message, new List<string> { ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDto)
        {
            try
            {
                var product = await _productService.UpdateProduct(productDto);
                return ResponseOk(product);
            }
            catch (Exception ex)
            {
                return ResponseError(ex.Message, new List<string> { ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return ResponseOk("Delete Success");
            }
            catch (Exception ex)
            {
                return ResponseError(ex.Message, new List<string> { ex.Message });
            }
        }
    }
}
