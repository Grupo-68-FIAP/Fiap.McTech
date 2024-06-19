using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Infra.Context;
using Fiap.McTech.Infra.Repositories.Cart;
using Microsoft.EntityFrameworkCore;

namespace Fiap.McTech.Infra.UnitTests.Repositories
{
    public sealed class CartClientRepositoryUnitTests : RepositoryBaseUnitTests<CartClient>
    {
        readonly Client client = new("Client test", new(""), new(""));

        readonly Product product1 = new("Product 1", 10, "", "", Domain.Enums.ProductCategory.None);
        readonly Product product2 = new("Product 2", 20, "", "", Domain.Enums.ProductCategory.Beverage);

        protected override CartClientRepository GetRepository(DataContext context)
        {
            return new CartClientRepository(context);
        }

        protected override CartClient MakeNewEntity()
        {
            if (_context == null)
                Assert.Fail("Context is null");

            var clients = _context.Set<Client>();
            clients.Add(client);

            var products = _context.Set<Product>();
            products.Add(product1);
            products.Add(product2);

            var cart = new CartClient(client.Id, 0);
            var cartItem = new CartClient.Item(product1.Name, 1, product1.Value, product1.Id, cart.Id);
            cart.Items.Add(cartItem);
            cart.CalculateValueCart();
            return cart;
        }

        [Fact]
        public async Task GetByCartIdAsync_WithValidId_ShouldReturnCart()
        {
            Before();
            if (_context == null)
                Assert.Fail("Context is null");

            var cart = MakeNewEntity();
            await _context.Set<CartClient>().AddAsync(cart);
            await _context.SaveChangesAsync();

            var repository = GetRepository(_context);
            var result = await repository.GetByCartIdAsync(cart.Id);

            Assert.NotNull(result);
            Assert.Equal(cart.Id, result.Id);
            Assert.Contains(product1, result.Items.Select(c => c.Product));

            After();
        }

        [Fact]
        public async Task GetByClientIdAsync_WithValidId_ShouldReturnCart()
        {
            Before();
            if (_context == null)
                Assert.Fail("Context is null");

            var cart = MakeNewEntity();
            await _context.Set<CartClient>().AddAsync(cart);
            await _context.SaveChangesAsync();

            var repository = GetRepository(_context);
            var result = await repository.GetByClientIdAsync(client.Id);

            Assert.NotNull(result);
            Assert.Equal(cart.Id, result.Id);
            Assert.Contains(product1, result.Items.Select(c => c.Product));

            After();
        }

        [Fact]
        public async Task UpdateAsync_AddProduct_Sucess()
        {
            Before();
            if (_context == null)
                Assert.Fail("Context is null");

            var cart = MakeNewEntity();
            await _context.Set<CartClient>().AddAsync(cart);
            await _context.SaveChangesAsync();
            _context.Entry(cart).State = EntityState.Detached;


            var repository = GetRepository(_context);
            var cartFromDb = await repository.GetByCartIdAsync(cart.Id);
            if (cartFromDb == null)
                Assert.Fail("Cart not found");
            cartFromDb.AddProduct(product2, 1);

            await repository.UpdateAsync(cartFromDb);

            var cartResult = await repository.GetByCartIdAsync(cart.Id);

            Assert.Equal(2, cartResult?.Items.Count);

            After();
        }
    }
}
