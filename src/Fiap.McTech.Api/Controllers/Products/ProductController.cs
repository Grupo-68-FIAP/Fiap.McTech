using Fiap.McTech.Application.Dtos.Products;
using Fiap.McTech.Application.Dtos.Products.Add;
using Fiap.McTech.Application.Dtos.Products.Update;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Fiap.McTech.Api.Controllers.Product
{
    /// <summary>
    /// Controller responsible for handling operations related to products.
    /// </summary>
    [ApiController]
    [Route("api/product")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ProductController : Controller
    {
        private readonly IProductAppService _productAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class with the specified product application service.
        /// </summary>
        /// <param name="productAppService">The service to manage product operations.</param>
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

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>A list of all products.</returns>
        /// <response code="200">Returns a list of products.</response>
        /// <response code="404">If no products are found.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ProductOutputDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productAppService.GetAllProductsAsync();
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }

            return Ok(products);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productDto">The data of the product to be created.</param>
        /// <returns>The created product.</returns>
        /// <response code="201">Returns the newly created product.</response>
        /// <response code="400">If there are validation issues.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProductOutputDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct(CreateProductInputDto productDto)
        {
            var createdProduct = await _productAppService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The unique identifier of the product to be updated.</param>
        /// <param name="productDto">The updated data of the product.</param>
        /// <returns>A result indicating the outcome of the update operation.</returns>
        /// <response code="200">If the product was successfully updated.</response>
        /// <response code="404">If the product does not exist.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductInputDto productDto)
        {
            var updatedProduct = await _productAppService.UpdateProductAsync(id, productDto);
            if (updatedProduct == null)
            {
                return NotFound("Product not found.");
            }

            return Ok();
        }

        /// <summary>
        /// Deletes a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to be deleted.</param>
        /// <returns>A result indicating the outcome of the delete operation.</returns>
        /// <response code="204">If the product was successfully deleted.</response>
        /// <response code="400">If there was an error deleting the product.</response>
        /// <response code="404">If the product does not exist.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <param name="category">
        /// Category to find products. Possible values are:
        /// <list type="bullet">
        /// <item><description><c>None</c> (-1): No specific category.</description></item>
        /// <item><description><c>Snack</c> (0): Category for snacks.</description></item>
        /// <item><description><c>SideDish</c> (1): Category for side dishes.</description></item>
        /// <item><description><c>Beverage</c> (2): Category for beverages.</description></item>
        /// <item><description><c>Dessert</c> (3): Category for desserts.</description></item>
        /// </list>
        /// </param>
        /// <returns>List of products in the specified category.</returns>
        /// <response code="200">Returns list of products.</response>
        /// <response code="204">If there are no products in the specified category.</response>
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