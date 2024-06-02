using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Infra.Repositories.Products;

namespace Fiap.McTech.Infra.UnitTests.Repositories
{
    public sealed class ProductRepositoryUnitTests : RepositoryBaseUnitTests<Product>
    {
        readonly static Product PRODUCT_1 = new("Product 1", 10, "desc 1", "img1", Domain.Enums.ProductCategory.None);
        readonly static Product PRODUCT_2 = new("Product 2", 100, "desc 2", "img2", Domain.Enums.ProductCategory.Beverage);
        readonly static Product PRODUCT_3 = new("Product 3", 1000, "desc 3", "img3", Domain.Enums.ProductCategory.Dessert);


        public ProductRepositoryUnitTests() : base(PRODUCT_1)
        {
            // Popule o banco de dados em memória com alguns dados de teste
            _context.Products?.Add(PRODUCT_1);
            _context.Products?.Add(PRODUCT_2);
            _context.SaveChanges();
        }

        protected override ProductRepository GetRepository()
        {
            return new ProductRepository(_context);
        }

        protected override Product GetEntityToAddTest()
        {
            return PRODUCT_3;
        }
    }
}
