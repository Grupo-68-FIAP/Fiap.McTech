using Microsoft.AspNetCore.Mvc;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.Dtos.Products.Update;
using Fiap.McTech.Application.Dtos.Products;
using Fiap.McTech.Application.Dtos.Clients;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Api.Controllers.Product
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        public readonly IProductAppService _productAppService;

        public ProductController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductOutputDto>> GetProduct(Guid id)
        {
            var product = await _productAppService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductOutputDto>>> GetAllProducts()
        {
            var products = await _productAppService.GetAllProductsAsync();
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }

            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<ProductOutputDto>> CreateProduct(ProductOutputDto productDto)
        {
            var createdProduct = await _productAppService.CreateProductAsync(productDto);
            if (createdProduct == null)
            {
                return BadRequest("Unable to create product.");
            }

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductInputDto productDto)
        {
            var updatedProduct = await _productAppService.UpdateProductAsync(id, productDto);
            if (updatedProduct == null)
            {
                return NotFound("Product not found.");
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var deleted = await _productAppService.DeleteProductAsync(id);
            if (!deleted.IsSuccess)
            {
                return BadRequest("Error to delete product");
            }

            return NoContent();
        }

        /// <summary>
        /// Obtain products by category.
        /// </summary>
        /// <returns>List of clients</returns>
        /// <param name="categoryId">Category to find products</param>
        /// <response code="200">Returns list of products</response>
        /// <response code="204">If there are nothing</response>
        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(List<ProductOutputDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetProductsByCategory(ProductCategory category)
        {
            var products = await _productAppService.GetProductsByCategoryAsync(category);
            return (products == null || !products.Any()) ? new NoContentResult() : Ok(products);
        }
    }
}