using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Infra.Context;
using Fiap.McTech.Infra.Repositories.Orders;

namespace Fiap.McTech.Infra.UnitTests.Repositories
{
    public sealed class OrderRepositoryUnitTests : RepositoryBaseUnitTests<Order>
    {
        readonly Client client = new("Client test", new(""), new(""));

        readonly Product product1 = new("Product 1", 10, "", "", Domain.Enums.ProductCategory.None);
        readonly Product product2 = new("Product 2", 20, "", "", Domain.Enums.ProductCategory.Beverage);

        protected override OrderRepository GetRepository(DataContext context)
        {
            return new OrderRepository(context);
        }

        protected override Order MakeNewEntity()
        {
            if (_context == null)
                Assert.Fail("Context is null");

            var clients = _context.Set<Client>();
            clients.Add(client);

            var products = _context.Set<Product>();
            products.Add(product1);
            products.Add(product2);

            var order = new Order(null, 0);
            var orderItem = new Order.Item(product1.Id, order.Id, product1.Name, product1.Value, 1);
            order.Items.Add(orderItem);
            return order;
        }

        [Fact]
        public async Task GetOrderByIdAsync_WithValidId_ShouldReturnOrder()
        {
            Before();
            if (_context == null)
                Assert.Fail("Context is null");

            var order = MakeNewEntity();
            await _context.Set<Order>().AddAsync(order);
            await _context.SaveChangesAsync();

            var repository = GetRepository(_context);
            var result = await repository.GetOrderByIdAsync(order.Id);

            Assert.NotNull(result);
            Assert.Equal(order.Id, result.Id);

            After();
        }

        [Fact]
        public async Task GetOrderByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            Before();
            if (_context == null)
                Assert.Fail("Context is null");

            var repository = GetRepository(_context);
            var result = await repository.GetOrderByIdAsync(Guid.NewGuid());

            Assert.Null(result);

            After();
        }

        [Fact]
        public async Task GetOrderByStatusAsync_WithValidStatus_ShouldReturnOrders()
        {
            Before();
            if (_context == null)
                Assert.Fail("Context is null");

            var order1 = MakeNewEntity();
            order1.SendToNextStatus(); // None -> Pending
            await _context.Set<Order>().AddAsync(order1);

            var order2 = MakeNewEntity();
            order2.SendToNextStatus();// None -> Pending
            order2.SendToNextStatus();// Pending -> Processing
            await _context.Set<Order>().AddAsync(order2);

            var order3 = MakeNewEntity();
            order3.SendToNextStatus();// None -> Pending
            order3.SendToNextStatus();// Pending -> Processing
            order3.SendToNextStatus();// Processing -> Completed
            await _context.Set<Order>().AddAsync(order3);

            await _context.SaveChangesAsync();

            var repository = GetRepository(_context);
            var result = await repository.GetOrderByStatusAsync(OrderStatus.Processing);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Single(result);

            After();
        }

        [Fact]
        public async Task GetOrderByStatusAsync_WithInvalidStatus_ShouldReturnEmptyList()
        {
            Before();
            if (_context == null)
                Assert.Fail("Context is null");

            var repository = GetRepository(_context);
            var result = await repository.GetOrderByStatusAsync(OrderStatus.Completed);

            Assert.NotNull(result);
            Assert.Empty(result);

            After();
        }

        [Fact]
        public async Task GetCurrrentOrders_WithOrders_ShouldReturnOrders()
        {
            Before();
            if (_context == null)
                Assert.Fail("Context is null");

            //Do not show completed orders
            var order1 = MakeNewEntity();
            order1.SendToNextStatus(); // None -> Pending
            order1.SendToNextStatus();// Pending -> Processing
            order1.SendToNextStatus();// Processing -> Completed
            await _context.Set<Order>().AddAsync(order1);

            // Second Item to show
            var order2 = MakeNewEntity();
            order2.SendToNextStatus();// None -> Pending
            await _context.Set<Order>().AddAsync(order2);
            await Task.Delay(1000);

            // Third Item to show
            var order3 = MakeNewEntity();
            order3.SendToNextStatus();// None -> Pending
            order3.SendToNextStatus();// Pending -> Processing
            await _context.Set<Order>().AddAsync(order3);
            await Task.Delay(1000);

            // First Item to show
            var order4 = MakeNewEntity();
            order4.SendToNextStatus();// None -> Pending
            await _context.Set<Order>().AddAsync(order4);
            await Task.Delay(1000);

            // Do not show canceled orders
            var order5 = MakeNewEntity();
            order5.Cancel();
            await _context.Set<Order>().AddAsync(order5);

            // Do not show none status orders
            var order6 = MakeNewEntity();
            await _context.Set<Order>().AddAsync(order6);

            await _context.SaveChangesAsync();

            var repository = GetRepository(_context);
            var result = await repository.GetCurrrentOrders();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(order4.Id, result[0].Id);
            Assert.Equal(order2.Id, result[1].Id);
            Assert.Equal(order3.Id, result[2].Id);
            Assert.DoesNotContain(order1.Id, result.Select(o => o.Id));
            Assert.DoesNotContain(order5.Id, result.Select(o => o.Id));
            Assert.DoesNotContain(order6.Id, result.Select(o => o.Id));

            After();
        }
    }
}
