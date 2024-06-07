using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Infra.Context;
using Fiap.McTech.Infra.Repositories.Products;

namespace Fiap.McTech.Infra.UnitTests.Repositories
{
    public sealed class ProductRepositoryUnitTests : RepositoryBaseUnitTests<Product>
    {
        protected override ProductRepository GetRepository(DataContext context)
        {
            return new ProductRepository(context);
        }

        protected override Product MakeNewEntity()
        {
            Random rand = new();
            var i = rand.Next(1000, 9999);
            return new($"Product Rand ${i}", 1000, $"desc rand ${i}", $"img{i}", Domain.Enums.ProductCategory.Dessert);
        }

        [Theory]
        [InlineData(ProductCategory.Dessert, 3)]
        [InlineData(ProductCategory.Snack, 2)]
        [InlineData(ProductCategory.Beverage, 1)]
        public async Task GetProductsByCategoryAsync_ShouldReturnProductsByCategory(ProductCategory category, int total)
        {
            Before();
            if (_context == null)
                Assert.Fail();

            // Arrange
            var repository = GetRepository(_context);
            var products = new List<Product>
            {
                new("Product 1", 1000, "desc 1", "img1", ProductCategory.Dessert),
                new("Product 2", 2000, "desc 2", "img2", ProductCategory.Dessert),
                new("Product 3", 3000, "desc 3", "img3", ProductCategory.Dessert),
                new("Product 4", 4000, "desc 4", "img4", ProductCategory.Snack),
                new("Product 5", 5000, "desc 5", "img5", ProductCategory.Snack),
                new("Product 6", 6000, "desc 6", "img6", ProductCategory.Beverage),
            };
            var dbSet = _context.Set<Product>();
            await dbSet.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            // Act
            var result = await repository.GetProductsByCategoryAsync(category);

            // Assert
            Assert.Equal(total, result.Count);
            Assert.All(result, p => Assert.Equal(category, p.Category));

            After();
        }
    }
}
