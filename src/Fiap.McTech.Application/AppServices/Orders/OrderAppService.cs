using AutoMapper;
using Fiap.McTech.Application.Dtos.Orders.Add;
using Fiap.McTech.Application.Dtos.Orders.Delete;
using Fiap.McTech.Application.Dtos.Orders.Update;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.ViewModels.Orders;
using Fiap.McTech.Domain.Interfaces.Repositories.Orders;
using Microsoft.Extensions.Logging;

namespace Fiap.McTech.Application.AppServices.Orders
{
    public class OrderAppService : IOrderAppService
    {
		private readonly ILogger<OrderAppService> _logger;
		private readonly IOrderRepository _orderRepository;
		private readonly IMapper _mapper;

		public OrderAppService(IOrderRepository orderRepository, IMapper mapper, ILogger<OrderAppService> logger)
		{
			_orderRepository = orderRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<OrderOutputDto?> GetOrderByIdAsync(Guid id)
		{
			try
			{
				_logger.LogInformation("Retrieving order with ID {OrderId}.", id);

				var order = await _orderRepository.GetByIdAsync(id);
				if (order == null)
				{
					_logger.LogInformation("Order with ID {OrderId} not found.", id);
					return null;
				}

				_logger.LogInformation("Order with ID {OrderId} retrieved successfully.", id);

				return _mapper.Map<OrderOutputDto>(order);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to retrieve order with ID {OrderId}.", id);
				throw;
			}
		}

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

		public async Task<OrderOutputDto> UpdateOrderAsync(Guid orderId, UpdateOrderInputDto orderDto)
		{
			try
			{
				var existingOrder = await _orderRepository.GetByIdAsync(orderId);
				if (existingOrder == null)
				{
					_logger.LogWarning("Order with ID {OrderId} not found. Update aborted.", orderId);
					throw new InvalidOperationException("Order not found.");
				}

				_logger.LogInformation("Updating order with ID {OrderId}.", orderId);

				_mapper.Map(orderDto, existingOrder);

				await _orderRepository.UpdateAsync(existingOrder);

				_logger.LogInformation("Order with ID {OrderId} updated successfully.", orderId);

				return _mapper.Map<OrderOutputDto>(existingOrder);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to update order with ID {OrderId}.", orderId);
				throw;
			}
		}

		public async Task<DeleteOrderOutputDto> DeleteOrderAsync(Guid orderId)
		{
			try
			{
				_logger.LogInformation("Attempting to delete order with ID: {OrderId}", orderId);

				var existingOrder = await _orderRepository.GetByIdAsync(orderId);
				if (existingOrder == null)
				{
					_logger.LogWarning("Order with ID {OrderId} not found. Deletion aborted.", orderId);

					return new DeleteOrderOutputDto(isSuccess: false, message: "Order not found.");
				}

				await _orderRepository.RemoveAsync(existingOrder);

				_logger.LogInformation("Order with ID {OrderId} deleted successfully.", orderId);

				return new DeleteOrderOutputDto(isSuccess: true, message: "Order deleted successfully.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to delete order with ID: {OrderId}", orderId);
				throw;
			}
		}
	}
}