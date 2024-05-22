using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Domain.Interfaces.Repositories.Orders
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<List<Order>> GetOrderByStatusAsync(OrderStatus status);
    }
}