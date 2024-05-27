using AutoMapper;
using Fiap.McTech.Application.Dtos.Orders.Add;
using Fiap.McTech.Application.Dtos.Orders.Update;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.ViewModels.Orders;
using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Cart;
using Fiap.McTech.Domain.Interfaces.Repositories.Orders;
using Fiap.McTech.Domain.Interfaces.Repositories.Payments;
using Fiap.McTech.Domain.Utils.Extensions;
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
            try
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
            catch (McTechException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve order with ID {OrderId}.", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<OrderOutputDto> CreateOrderAsync(CreateOrderInputDto orderDto)
        {
            try
            {
                _logger.LogInformation("Creating a new order.");

                var order = _mapper.Map<Domain.Entities.Orders.Order>(orderDto);

                var createdOrder = await _orderRepository.AddAsync(order);

                _logger.LogInformation("Order created successfully with ID {OrderId}.", createdOrder.Id);

                return _mapper.Map<OrderOutputDto>(createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create order");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<OrderOutputDto> CreateOrderByCartAsync(Guid cartId)
        {
            try
            {
                _logger.LogInformation("Creating a new order by cart id {cartId}.", cartId);
                var cart = await _cartClientRepository.GetByCartIdAsync(cartId);
                if (cart == null)
                {
                    _logger.LogWarning("Cart with ID {cartId} not found", cartId);
                    throw new EntityNotFoundException(string.Format("Cart with ID {0} not found.", cartId));
                }

                var order = _mapper.Map<Order>(cart);

                var createdOrder = await _orderRepository.AddAsync(order);
                _logger.LogInformation("Order created successfully with ID {OrderId}.", createdOrder.Id);

                await _cartClientRepository.RemoveAsync(cart);

                return _mapper.Map<OrderOutputDto>(createdOrder);
            }
            catch (McTechException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create order");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task DeleteOrderAsync(Guid orderId)
        {
            try
            {
                _logger.LogInformation("Attempting to delete order with ID: {OrderId}", orderId);

                var existingOrder = await _orderRepository.GetByIdAsync(orderId)
                    ?? throw new EntityNotFoundException(string.Format("Order with ID {0} not found.", orderId));

                await _orderRepository.RemoveAsync(existingOrder);

                _logger.LogInformation("Order with ID {OrderId} deleted successfully.", orderId);
            }
            catch (McTechException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete order with ID: {OrderId}", orderId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<OrderOutputDto>> GetOrderByStatusAsync(OrderStatus status)
        {
            try
            {
                _logger.LogInformation("Retrieving order with status code {status}.", status);

                var orders = await _orderRepository.GetOrderByStatusAsync(status);
                if (orders == null)
                {
                    _logger.LogInformation("Order with status code {status} not found.", status);
                    throw new EntityNotFoundException(string.Format("Order with status code {0} not found.", status));
                }

                _logger.LogInformation("Order with status code {status} retrieved successfully.", status);

                return _mapper.Map<List<OrderOutputDto>>(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve order with status code {status}.", status);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<OrderOutputDto> MoveOrderToNextStatus(Guid id)
        {
            try
            {
                var originalOrder = await _orderRepository.GetByIdAsync(id);
                if (originalOrder == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found. Update aborted.", id);
                    throw new EntityNotFoundException(string.Format("Order with ID {0} not found. Update aborted.", id));
                }
                if (originalOrder.Status == OrderStatus.Pending)
                {
                    var payment = await _paymentRepository.GetByOrderIdAsync(id)
                        ?? throw new PaymentRequiredException("Waiting for payment.");
                }

                var modifierOrder = _mapper.Map<UpdateOrderInputDto>(originalOrder);
                modifierOrder.Status = originalOrder.Status.NextStatus();

                _mapper.Map(modifierOrder, originalOrder);

                await _orderRepository.UpdateAsync(originalOrder);

                return _mapper.Map<OrderOutputDto>(await _orderRepository.GetOrderByIdAsync(id));
            }
            catch (McTechException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update order with ID {OrderId}.", id);
                throw;
            }
        }
    }
}
