using Fiap.McTech.Application.Dtos.Products.Update;
using Fiap.McTech.Application.Dtos.Products;
using Fiap.McTech.Application.Dtos.Products.Delete;

namespace Fiap.McTech.Application.Interfaces
{
	public interface IProductAppService
	{
		Task<ProductOutputDto> GetProductByIdAsync(Guid productId);
		Task<List<ProductOutputDto>> GetAllProductsAsync();
		Task<ProductOutputDto> CreateProductAsync(ProductOutputDto productDto);
		Task<UpdateProductOutputDto> UpdateProductAsync(Guid productId, UpdateProductInputDto productDto);
		Task<DeleteProductOutputDto> DeleteProductAsync(Guid productId);
	}
}