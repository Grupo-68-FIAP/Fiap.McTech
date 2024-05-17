using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Interfaces.Repositories.Products;
using Fiap.McTech.Domain.ValuesObjects;
using Fiap.McTech.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Fiap.McTech.Infra.Repositories.Products
{
	public class ProductRepository : RepositoryBase<Product>, IProductRepository
	{
		public ProductRepository(DataContext context) : base(context) { }
        public async Task<List<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _dbSet.Where(p => category.Equals(p.Category)).ToListAsync();
        }

    }
}
