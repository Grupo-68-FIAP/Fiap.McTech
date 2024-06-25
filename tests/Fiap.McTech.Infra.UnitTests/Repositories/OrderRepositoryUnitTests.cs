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
            order1.SendToNextStatus(); // None -> WaitPayment
            await _context.Set<Order>().AddAsync(order1);

            var order2 = MakeNewEntity();
            order2.SendToNextStatus();// None -> WaitPayment
            order2.SendToNextStatus();// WaitPayment -> Received
            await _context.Set<Order>().AddAsync(order2);

            var order3 = MakeNewEntity();
            order3.SendToNextStatus();// None -> WaitPayment
            order3.SendToNextStatus();// WaitPayment -> Received
            order3.SendToNextStatus();// Received -> InPreparation
            await _context.Set<Order>().AddAsync(order3);

            await _context.SaveChangesAsync();

            var repository = GetRepository(_context);
            var result = await repository.GetOrderByStatusAsync(OrderStatus.Received);

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
            var result = await repository.GetOrderByStatusAsync(OrderStatus.Finished);

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

            // Third Item to show
            var order_inPreparation_2 = MakeNewEntity();
            order_inPreparation_2.SendToNextStatus();// None -> WaitPayment
            order_inPreparation_2.SendToNextStatus();// WaitPayment -> Received
            order_inPreparation_2.SendToNextStatus();// Received -> InPreparation
            await _context.Set<Order>().AddAsync(order_inPreparation_2);

            // Fourth Item to show
            await Task.Delay(1000);
            var order_received = MakeNewEntity();
            order_received.SendToNextStatus();// None -> WaitPayment
            order_received.SendToNextStatus();// WaitPayment -> Received
            await _context.Set<Order>().AddAsync(order_received);

            // First Item to show
            await Task.Delay(1000);
            var order_ready = MakeNewEntity();
            order_ready.SendToNextStatus();// None -> WaitPayment
            order_ready.SendToNextStatus();// WaitPayment -> Received
            order_ready.SendToNextStatus();// Received -> InPreparation
            order_ready.SendToNextStatus();// InPreparation -> Ready
            await _context.Set<Order>().AddAsync(order_ready);

            // Second Item to show
            await Task.Delay(1000);
            var order_inPreparation_1 = MakeNewEntity();
            order_inPreparation_1.SendToNextStatus();// None -> WaitPayment
            order_inPreparation_1.SendToNextStatus();// WaitPayment -> Received
            order_inPreparation_1.SendToNextStatus();// Received -> InPreparation
            await _context.Set<Order>().AddAsync(order_inPreparation_1);

            // Do not show no payed orders
            await Task.Delay(1000);
            var order_waitPayment = MakeNewEntity();
            order_waitPayment.SendToNextStatus();// None -> WaitPayment
            await _context.Set<Order>().AddAsync(order_waitPayment);

            // Do not show canceled orders
            var order_canceled = MakeNewEntity();
            order_canceled.Cancel();
            await _context.Set<Order>().AddAsync(order_canceled);

            // Do not show none status orders
            var order_noned = MakeNewEntity();
            await _context.Set<Order>().AddAsync(order_noned);

            await _context.SaveChangesAsync();

            var repository = GetRepository(_context);
            var result = await repository.GetCurrrentOrders();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(4, result.Count);
            Assert.DoesNotContain(order_waitPayment.Id, result.Select(o => o.Id));
            Assert.DoesNotContain(order_canceled.Id, result.Select(o => o.Id));
            Assert.DoesNotContain(order_noned.Id, result.Select(o => o.Id));
            // Verify order
            Assert.Equal(order_ready.Id, result[0].Id);
            Assert.Equal(order_inPreparation_1.Id, result[1].Id);
            Assert.Equal(order_inPreparation_2.Id, result[2].Id);
            Assert.Equal(order_received.Id, result[3].Id);

            After();
        }
    }
}
