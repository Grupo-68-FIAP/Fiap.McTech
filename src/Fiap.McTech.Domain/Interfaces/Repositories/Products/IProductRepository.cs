using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Domain.Interfaces.Repositories.Products
{
    /// <summary>
    /// Represents a repository interface for CRUD operations with products in the McTech domain.
    /// </summary>
    public interface IProductRepository : IRepositoryBase<Product>
    {
        /// <summary>
        /// Asynchronously retrieves products by their category.
        /// </summary>
        /// <param name="category">The category of products to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of products.</returns>
        Task<List<Product>> GetProductsByCategoryAsync(ProductCategory category);
    }
}
