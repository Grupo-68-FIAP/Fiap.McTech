using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Interfaces.Repositories.Products;
using Fiap.McTech.Domain.ValuesObjects;
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Fiap.McTech.Infra.Repositories.Products
{
	public class ProductRepository : RepositoryBase<Product>, IProductRepository
	{
		public ProductRepository(DataContext context) : base(context) { }
        public async Task<List<Product>> GetProductsByCategoryAsync(ProductCategory category)
        {
            return await _dbSet.Where(p => p.Category == category).ToListAsync();
        }

    }
}
