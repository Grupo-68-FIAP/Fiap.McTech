using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Domain.Interfaces.Repositories.Products
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<List<Product>> GetProductsByCategoryAsync(ProductCategory category);
    }
}