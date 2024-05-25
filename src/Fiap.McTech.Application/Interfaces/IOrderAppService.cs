using Fiap.McTech.Application.ViewModels.Orders;
using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Application.Interfaces
{
    /// <summary>
    /// Interface for Order Application Service.
    /// </summary>
    public interface IOrderAppService
    {
        /// <summary>
        /// Retrieves an order by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>A task representing the asynchronous operation that returns the order output DTO.</returns>
        Task<OrderOutputDto?> GetOrderByIdAsync(Guid id);

        /// <summary>
        /// Retrieves orders by their status asynchronously.
        /// </summary>
        /// <param name="status">The status of the orders to retrieve.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of order output DTOs.</returns>
        Task<List<OrderOutputDto>> GetOrderByStatusAsync(OrderStatus status);

        /// <summary>
        /// Creates an order based on a cart asynchronously.
        /// </summary>
        /// <param name="cartId">The ID of the cart from which to create the order.</param>
        /// <returns>A task representing the asynchronous operation that returns the created order output DTO.</returns>
        Task<OrderOutputDto> CreateOrderByCartAsync(Guid cartId);

        /// <summary>
        /// Deletes an order asynchronously.
        /// </summary>
        /// <param name="orderId">The ID of the order to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteOrderAsync(Guid orderId);

        /// <summary>
        /// Moves an order to the next status asynchronously.
        /// </summary>
        /// <param name="id">The ID of the order to move to the next status.</param>
        /// <returns>A task representing the asynchronous operation that returns the updated order output DTO.</returns>
        Task<OrderOutputDto> MoveOrderToNextStatus(Guid id);
    }
}