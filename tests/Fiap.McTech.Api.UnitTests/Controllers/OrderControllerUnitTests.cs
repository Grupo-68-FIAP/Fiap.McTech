using AutoMapper;
using Fiap.McTech.Api.Controllers.Orders;
using Fiap.McTech.Application.AppServices.Orders;
using Fiap.McTech.Application.ViewModels.Orders;
using Fiap.McTech.CrossCutting.Ioc.Mappers.Profiles;
using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Cart;
using Fiap.McTech.Domain.Interfaces.Repositories.Orders;
using Fiap.McTech.Domain.Interfaces.Repositories.Payments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fiap.McTech.Api.UnitTests.Controllers
{
    public class OrderControllerUnitTests
    {
        private readonly Mock<ICartClientRepository> _mockedCartClientRepository;
        private readonly Mock<IPaymentRepository> _mockedPaymentRepository;
        private readonly Mock<IOrderRepository> _mockedOrderRepository;

        private readonly Mock<ILogger<OrderAppService>> _mockedLogger;
        private readonly IMapper _mapper;

        private readonly Product product = new("Product", 10, "Description", "Image", Domain.Enums.ProductCategory.None);

        public OrderControllerUnitTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CartClientProfile>();
                cfg.AddProfile<ClientProfile>();
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<OrderProfile>();
            });
            _mapper = new Mapper(configuration);
            _mockedLogger = new Mock<ILogger<OrderAppService>>();

            _mockedCartClientRepository = new Mock<ICartClientRepository>();
            _mockedPaymentRepository = new Mock<IPaymentRepository>();
            _mockedOrderRepository = new Mock<IOrderRepository>();
        }

        [Fact]
        public async Task GetOrderById_WithValidId_ReturnsOrder()
        {
            // Arrange
            var order = new Order(null, 0);
            order.Items.Add(new Order.Item(product.Id, order.Id, product.Name, product.Value, 1));
            _mockedOrderRepository
                .Setup(x => x.GetOrderByIdAsync(order.Id))
                .ReturnsAsync(order);
            var controller = CreateOrderController();

            // Act
            var result = await controller.GetOrderById(order.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var orderOutputDto = Assert.IsType<OrderOutputDto>(okResult.Value);
            Assert.Equal(order.Id, orderOutputDto.Id);
            Assert.Equal(order.Status, orderOutputDto.Status);
            Assert.Equal(order.Items.Count, orderOutputDto.Items.Count);
        }

        [Fact]
        public async Task GetOrderById_Throws_EntityNotFoundException()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var controller = CreateOrderController();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.GetOrderById(guid));
            Assert.Contains(guid.ToString(), exception.Message);
            Assert.Contains("Order", exception.Message);
        }

        [Fact]
        public async Task CreateOrder_WithValidCartId_ReturnsCreatedOrder()
        {
            // Arrange
            var cart = new CartClient(null, 0);
            cart.Items.Add(new CartClient.Item(product.Name, 1, product.Value, product.Id, cart.Id));
            _mockedOrderRepository
                .Setup(repository => repository.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync<Order, IOrderRepository, Order>(x => x);
            _mockedCartClientRepository
                .Setup(x => x.GetByCartIdAsync(cart.Id))
                .ReturnsAsync(cart);
            var controller = CreateOrderController();

            // Act
            var result = await controller.CreateOrder(cart.Id);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var orderOutputDto = Assert.IsType<OrderOutputDto>(createdResult.Value);
            Assert.Equal(OrderStatus.None, orderOutputDto.Status);
            Assert.True(orderOutputDto.Items.Any());
            Assert.Equal(cart.AllValue, orderOutputDto.TotalAmount);
            _mockedOrderRepository.Verify(x => x.AddAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrder_Throws_EntityNotFoundException()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var controller = CreateOrderController();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.CreateOrder(guid));
            Assert.Contains(guid.ToString(), exception.Message);
            Assert.Contains("Cart", exception.Message);
            _mockedOrderRepository.Verify(x => x.AddAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOrder_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var order = new Order(null, 0);
            _mockedOrderRepository
                .Setup(x => x.GetByIdAsync(order.Id))
                .ReturnsAsync(order);
            _mockedOrderRepository
                .Setup(x => x.RemoveAsync(order))
                .Returns(Task.CompletedTask);
            var controller = CreateOrderController();

            // Act
            var result = await controller.DeleteOrder(order.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockedOrderRepository.Verify(x => x.RemoveAsync(order), Times.Once);
        }

        [Fact]
        public async Task DeleteOrder_Throws_EntityNotFoundException()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var controller = CreateOrderController();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.DeleteOrder(guid));
            Assert.Contains(guid.ToString(), exception.Message);
            Assert.Contains("Order", exception.Message);
            _mockedOrderRepository.Verify(x => x.RemoveAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task GetOrderByStatus_WithValidStatus_ReturnsOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new (null, 10),
                new (null, 20)
            };
            orders.ForEach(order => order.Items.Add(new Order.Item(product.Id, order.Id, product.Name, product.Value, 1)));
            _mockedOrderRepository
                .Setup(x => x.GetOrderByStatusAsync(Domain.Enums.OrderStatus.None))
                .ReturnsAsync(orders);
            var controller = CreateOrderController();

            // Act
            var result = await controller.GetOrderByStatus(Domain.Enums.OrderStatus.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var orderOutputDtos = Assert.IsType<List<OrderOutputDto>>(okResult.Value);
            Assert.Equal(2, orderOutputDtos.Count);
            Assert.All(orderOutputDtos, orderOutputDto => Assert.Equal(OrderStatus.None, orderOutputDto.Status));
            _mockedOrderRepository.Verify(x => x.GetOrderByStatusAsync(Domain.Enums.OrderStatus.None), Times.Once);
        }

        [Fact]
        public async Task GetOrderByStatus_WithNoOrders_ReturnsNoContentResult()
        {
            // Arrange
            _mockedOrderRepository
                .Setup(x => x.GetOrderByStatusAsync(Domain.Enums.OrderStatus.None))
                .ReturnsAsync(() => new());
            var controller = CreateOrderController();

            // Act
            var result = await controller.GetOrderByStatus(Domain.Enums.OrderStatus.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockedOrderRepository.Verify(x => x.GetOrderByStatusAsync(Domain.Enums.OrderStatus.None), Times.Once);
        }

        [Fact]
        public async Task MoveOrderToNextStatus_WithValidId_ReturnsOrder()
        {
            // Arrange
            var order = new Order(null, 0);
            _mockedOrderRepository
                .Setup(x => x.GetByIdAsync(order.Id))
                .ReturnsAsync(() => order);
            _mockedOrderRepository
                .Setup(x => x.GetOrderByIdAsync(order.Id))
                .ReturnsAsync(() => order);
            var controller = CreateOrderController();
            Assert.Equal(OrderStatus.None, order.Status);

            // Act
            var result = await controller.MoveOrderToNextStatus(order.Id);

            // Assert
            var resultObj = Assert.IsType<OkObjectResult>(result);
            var updatedOrder = Assert.IsType<OrderOutputDto>(resultObj.Value);
            Assert.Equal(OrderStatus.Pending, updatedOrder.Status);
            _mockedOrderRepository.Verify(x => x.UpdateAsync(It.Is<Order>(x => x.Status == OrderStatus.Pending)));
        }

        [Fact]
        public async Task MoveOrderToNextStatus_Throws_EntityNotFoundException()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var controller = CreateOrderController();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.MoveOrderToNextStatus(guid));
            Assert.Contains(guid.ToString(), exception.Message);
            Assert.Contains("Order", exception.Message);
            _mockedOrderRepository.Verify(x => x.UpdateAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task MoveOrderToNextStatus_Throws_PaymentRequiredException()
        {
            // Arrange
            var order = new Order(null, 0);
            order.SendToNextStatus();
            _mockedOrderRepository
                .Setup(x => x.GetByIdAsync(order.Id))
                .ReturnsAsync(() => order);
            var controller = CreateOrderController();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<PaymentRequiredException>(() => controller.MoveOrderToNextStatus(order.Id));
            Assert.Contains("payment", exception.Message);
            _mockedOrderRepository.Verify(x => x.UpdateAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task GetOrders_ReturnsOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new (null, 10),
                new (null, 20)
            };
            orders.ForEach(order => order.Items.Add(new Order.Item(product.Id, order.Id, product.Name, product.Value, 1)));
            _mockedOrderRepository
                .Setup(x => x.GetCurrrentOrders())
                .ReturnsAsync(orders);
            var controller = CreateOrderController();

            // Act
            var result = await controller.GetOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var orderOutputDtos = Assert.IsType<List<OrderOutputDto>>(okResult.Value);
            Assert.Equal(2, orderOutputDtos.Count);
            _mockedOrderRepository.Verify(x => x.GetCurrrentOrders(), Times.Once);
        }

        [Fact]
        public async Task GetOrders_WithNoOrders_ReturnsNoContentResult()
        {
            // Arrange
            _mockedOrderRepository
                .Setup(x => x.GetCurrrentOrders())
                .ReturnsAsync(() => new());
            var controller = CreateOrderController();

            // Act
            var result = await controller.GetOrders();

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockedOrderRepository.Verify(x => x.GetCurrrentOrders(), Times.Once);
        }

        private OrderController CreateOrderController()
        {
            var service = new OrderAppService(_mockedOrderRepository.Object, _mockedCartClientRepository.Object, _mockedPaymentRepository.Object, _mapper, _mockedLogger.Object);
            return new OrderController(service);
        }
    }
}
