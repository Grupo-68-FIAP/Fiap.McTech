using Fiap.McTech.Application.Dtos.Orders.Add;
using Fiap.McTech.Application.Dtos.Orders.Delete;
using Fiap.McTech.Application.Dtos.Orders.Update;
using Fiap.McTech.Application.ViewModels.Orders;
using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Application.Interfaces
{
	public interface IOrderAppService
	{
		Task<OrderOutputDto?> GetOrderByIdAsync(Guid id);
		Task<OrderOutputDto> CreateOrderByCartAsync(Guid cartId);
		Task<DeleteOrderOutputDto> DeleteOrderAsync(Guid orderId);
        Task<List<OrderOutputDto>> GetOrderByStatusAsync(OrderStatus status);
        Task<OrderOutputDto> MoveOrderToNextStatus(Guid id);
    }
}