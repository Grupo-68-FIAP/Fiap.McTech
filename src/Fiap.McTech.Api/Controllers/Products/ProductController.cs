using Microsoft.AspNetCore.Mvc;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.Dtos.Products.Update;
using Fiap.McTech.Application.Dtos.Products;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Application.Dtos.Products.Add;
using System.Net.Mime;
using Fiap.McTech.Domain.ValuesObjects;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Application.Dtos.Clients;

namespace Fiap.McTech.Api.Controllers.Product
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ProductController : Controller
    {
        public readonly IProductAppService _productAppService;

        public ProductController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        /// <summary>
        /// Obtain product by id
        /// </summary>
        /// <param name="id">Guid of reference that product</param>
        /// <returns>Return product</returns>
        /// <response code="200">Returns item</response>
        /// <response code="404">If product isn't exists</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductOutputDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductOutputDto>> GetProduct(Guid id)
        {
            try
            {
                var product = await _productAppService.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }
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

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="client">Input data of product</param>
        /// <returns>Return product</returns>
        /// <response code="201">Return new product</response>
        /// <response code="400">If there validations issues</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProductOutputDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct(CreateProductInputDto productDto)
        {
            var createdProduct = await _productAppService.CreateProductAsync(productDto);
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