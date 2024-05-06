using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Interfaces.Repositories.Products;
using Fiap.McTech.Infra.Context;

namespace Fiap.McTech.Infra.Repositories.Products
{
	public class ProductRepository : RepositoryBase<Product>, IProductRepository
	{
		public ProductRepository(DataContext context) : base(context) { }
	}
}
