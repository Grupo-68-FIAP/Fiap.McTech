using Fiap.McTech.Application.Dtos.Orders.Add;
using Fiap.McTech.Application.Dtos.Orders.Delete;
using Fiap.McTech.Application.Dtos.Orders.Update;
using Fiap.McTech.Application.ViewModels.Orders;

namespace Fiap.McTech.Application.Interfaces
{
	public interface IOrderAppService
	{
		Task<OrderOutputDto?> GetOrderByIdAsync(Guid id);
		Task<OrderOutputDto> CreateOrderAsync(CreateOrderInputDto orderDto);
		Task<OrderOutputDto> CreateOrderByCartAsync(Guid cartId);
        Task<OrderOutputDto> UpdateOrderAsync(Guid orderId, UpdateOrderInputDto orderDto);
		Task<DeleteOrderOutputDto> DeleteOrderAsync(Guid orderId);
	}
}