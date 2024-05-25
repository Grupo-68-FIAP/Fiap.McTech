using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Domain.Interfaces.Repositories.Orders
{
    /// <summary>
    /// Represents a repository interface for CRUD operations with orders in the McTech domain.
    /// </summary>
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        /// <summary>
        /// Asynchronously retrieves an order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <returns>A task representing the asynchronous operation, containing the order with the specified identifier, if found; otherwise, null.</returns>
        Task<Order?> GetOrderByIdAsync(Guid id);

        /// <summary>
        /// Asynchronously retrieves orders by their status.
        /// </summary>
        /// <param name="status">The status of orders to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of orders with the specified status.</returns>
        Task<List<Order>> GetOrderByStatusAsync(OrderStatus status);
    }
}
