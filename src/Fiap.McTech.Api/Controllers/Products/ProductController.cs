using Microsoft.AspNetCore.Mvc;
using Fiap.McTech.Application.Interfaces; 
using Fiap.McTech.Application.Dtos.Products.Update;
using Fiap.McTech.Application.Dtos.Products;

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
			try
			{
				var product = await _productAppService.GetProductByIdAsync(id);
				if (product == null)
				{
					return NotFound(); 
				}

				return Ok(product); 
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message); 
			}
		}

		[HttpGet]
		public async Task<ActionResult<List<ProductOutputDto>>> GetAllProducts()
		{
			try
			{
				var products = await _productAppService.GetAllProductsAsync();
				return Ok(products);  
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message); 
			}
		}

		[HttpPost]
		public async Task<ActionResult<ProductOutputDto>> CreateProduct(ProductOutputDto productDto)
		{
			try
			{
				var createdProduct = await _productAppService.CreateProductAsync(productDto);
				return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ProductId }, createdProduct);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message); 
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductInputDto productDto)
		{
			try
			{
				var updatedProduct = await _productAppService.UpdateProductAsync(id, productDto);
				if (updatedProduct == null)
				{
					return NotFound(); 
				}

				return Ok(); 
			}
			catch (InvalidOperationException)
			{
				return NotFound();  
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message); 
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(Guid id)
		{
			try
			{
				await _productAppService.DeleteProductAsync(id);

				return NoContent(); 
			}
			catch (InvalidOperationException)
			{
				return NotFound(); 
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message); 
			}
		}
	}
}