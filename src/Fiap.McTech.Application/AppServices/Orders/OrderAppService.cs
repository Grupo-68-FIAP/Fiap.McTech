using AutoMapper;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.ViewModels.Orders;
using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Cart;
using Fiap.McTech.Domain.Interfaces.Repositories.Orders;
using Fiap.McTech.Domain.Interfaces.Repositories.Payments;
using Microsoft.Extensions.Logging;

namespace Fiap.McTech.Application.AppServices.Orders
{
    /// <summary>
    /// Application service for handling order-related operations.
    /// </summary>
    public class OrderAppService : IOrderAppService
    {
        private readonly ILogger<OrderAppService> _logger;
        private readonly ICartClientRepository _cartClientRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderAppService"/> class.
        /// </summary>
        /// <param name="orderRepository">The repository for managing orders.</param>
        /// <param name="cartClientRepository">The repository for managing cart clients.</param>
        /// <param name="paymentRepository">The repository for managing payments.</param>
        /// <param name="mapper">The AutoMapper instance for object mapping.</param>
        /// <param name="logger">The logger instance for logging.</param>
        public OrderAppService(
            IOrderRepository orderRepository,
            ICartClientRepository cartClientRepository,
            IPaymentRepository paymentRepository,
            IMapper mapper,
            ILogger<OrderAppService> logger)
        {
            _orderRepository = orderRepository;
            _cartClientRepository = cartClientRepository;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<OrderOutputDto?> GetOrderByIdAsync(Guid id)
        {
            _logger.LogInformation("Retrieving order with ID {OrderId}.", id);

            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                _logger.LogInformation("Order with ID {OrderId} not found.", id);
                throw new EntityNotFoundException(string.Format("Order with ID {0} not found.", id));
            }

            _logger.LogInformation("Order with ID {OrderId} retrieved successfully.", id);

            return _mapper.Map<OrderOutputDto>(order);
        }

        /// <inheritdoc/>
        public async Task<OrderOutputDto> CreateOrderByCartAsync(Guid cartId)
        {
            _logger.LogInformation("Creating a new order by cart id {CartId}.", cartId);
            var cart = await _cartClientRepository.GetByCartIdAsync(cartId);
            if (cart == null)
            {
                _logger.LogWarning("Cart with ID {CartId} not found", cartId);
                throw new EntityNotFoundException(string.Format("Cart with ID {0} not found.", cartId));
            }

            var order = _mapper.Map<Order>(cart);

            var createdOrder = await _orderRepository.AddAsync(order);
            _logger.LogInformation("Order created successfully with ID {OrderId}.", createdOrder.Id);

            await _cartClientRepository.RemoveAsync(cart);

            return _mapper.Map<OrderOutputDto>(createdOrder);
        }

        /// <inheritdoc/>
        public async Task DeleteOrderAsync(Guid orderId)
        {
            _logger.LogInformation("Attempting to delete order with ID: {OrderId}", orderId);

            var existingOrder = await _orderRepository.GetByIdAsync(orderId)
                ?? throw new EntityNotFoundException(string.Format("Order with ID {0} not found.", orderId));

            await _orderRepository.RemoveAsync(existingOrder);

            _logger.LogInformation("Order with ID {OrderId} deleted successfully.", orderId);
        }

        /// <inheritdoc/>
        public async Task<List<OrderOutputDto>> GetOrderByStatusAsync(OrderStatus status)
        {
            _logger.LogInformation("Retrieving order with status code {Status}.", status);

            var orders = await _orderRepository.GetOrderByStatusAsync(status);
            if (!orders.Any())
                return new List<OrderOutputDto>();

            _logger.LogInformation("Order with status code {Status} retrieved successfully.", status);

            return _mapper.Map<List<OrderOutputDto>>(orders);
        }

        /// <inheritdoc/>
        public async Task<OrderOutputDto> MoveOrderToNextStatus(Guid id)
        {
            var originalOrder = await _orderRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException(string.Format("Order with ID {0} not found. Update aborted.", id));

            if (originalOrder.Status == OrderStatus.WaitPayment)
                _ = await _paymentRepository.GetByOrderIdAsync(id)
                    ?? throw new PaymentRequiredException("Waiting for payment.");

            originalOrder.SendToNextStatus();

            await _orderRepository.UpdateAsync(originalOrder);

            return _mapper.Map<OrderOutputDto>(await _orderRepository.GetOrderByIdAsync(id));
        }

        /// <inheritdoc/>
        public async Task<List<OrderOutputDto>> GetCurrrentOrders()
        {
            _logger.LogInformation("Retrieving all orders.");

            var orders = await _orderRepository.GetCurrrentOrders();

            if (orders == null || !orders.Any())
                return new List<OrderOutputDto>();

            _logger.LogInformation("All orders retrieved successfully.");

            return _mapper.Map<List<OrderOutputDto>>(orders);
        }
    }
}
