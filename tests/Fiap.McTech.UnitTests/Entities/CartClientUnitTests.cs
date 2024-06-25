using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Exceptions;

namespace Fiap.McTech.UnitTests.Entities
{
    public class CartClientUnitTests
    {
        [Fact]
        public void CartClient_WhenAddProduct_ShouldAddProductToCart()
        {
            // Arrange
            var cartClient = new CartClient(Guid.NewGuid(), 0);
            var product = new Product("Product 1", 10, "Desc", "img", ProductCategory.Snack);

            // Act
            cartClient.AddProduct(product, 1);

            // Assert
            Assert.Single(cartClient.Items);
            Assert.Equal(1, cartClient.Items.First().Quantity);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, -2)]
        public void CartClient_WhenAddProductWithInvalidQuantity_ShouldThrowException(int qt1, int qt2)
        {
            // Arrange
            var cartClient = new CartClient(Guid.NewGuid(), 0);
            var product1 = new Product("Product 1", 10, "Desc 1", "img", ProductCategory.Snack);
            if (qt1 > 0)
                cartClient.AddProduct(product1, qt1);

            // Act
            void act() => cartClient.AddProduct(product1, qt2);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(act);
            Assert.Equal("Invalid quantity.", exception.Message);
        }

        [Fact]
        public void CartClient_WhenAddProductWithExistingProduct_ShouldIncrementQuantity()
        {
            // Arrange
            var cartClient = new CartClient(Guid.NewGuid(), 0);
            var product = new Product("Product 1", 10, "Desc", "img", ProductCategory.Snack);

            // Act
            cartClient.AddProduct(product, 1);
            cartClient.AddProduct(product, 2);

            // Assert
            Assert.Single(cartClient.Items);
            Assert.Equal(3, cartClient.Items.First().Quantity);
        }

        [Fact]
        public void CartClient_WhenAddProductWithZeroQuantity_ShouldRemoveProductFromCart()
        {
            // Arrange
            var cartClient = new CartClient(Guid.NewGuid(), 0);
            var product = new Product("Product 1", 10, "Desc", "img", ProductCategory.Snack);

            // Act
            cartClient.AddProduct(product, 1);
            cartClient.AddProduct(product, -1);

            // Assert
            Assert.Empty(cartClient.Items);
        }

        [Fact]
        public void CartClient_WhenAddProductWithTwoDiferenteProducts_ShouldAddProductToCart()
        {
            // Arrange
            var cartClient = new CartClient(Guid.NewGuid(), 0);
            var product1 = new Product("Product 1", 10, "Desc 1", "img", ProductCategory.Snack);
            var product2 = new Product("Product 2", 10, "Desc 2", "img", ProductCategory.Snack);

            // Act
            cartClient.AddProduct(product1, 1);
            cartClient.AddProduct(product2, 1);

            // Assert
            Assert.Equal(2, cartClient.Items.Count);
        }
    }
}
