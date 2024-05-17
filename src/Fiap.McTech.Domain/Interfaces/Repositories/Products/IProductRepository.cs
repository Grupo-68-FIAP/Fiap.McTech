using Fiap.McTech.Domain.Entities.Products;

namespace Fiap.McTech.Domain.Interfaces.Repositories.Products
{
	public interface IProductRepository : IRepositoryBase<Product>
	{
        Task<List<Product>> GetProductsByCategoryAsync(string category);
    }
}