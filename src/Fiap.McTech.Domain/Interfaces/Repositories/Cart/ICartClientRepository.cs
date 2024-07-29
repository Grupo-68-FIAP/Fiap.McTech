using Fiap.McTech.Domain.Entities.Cart;

namespace Fiap.McTech.Domain.Interfaces.Repositories.Cart
{
    /// <summary>
    /// Represents a repository interface for CRUD operations with cart clients in the McTech domain.
    /// </summary>
    public interface ICartClientRepository : IRepositoryBase<CartClient>
    {
        /// <summary>
        /// Asynchronously retrieves a cart client by their cart identifier.
        /// </summary>
        /// <param name="id">The identifier of the cart.</param>
        /// <returns>A task representing the asynchronous operation, containing the cart client with the specified cart identifier, if found; otherwise, null.</returns>
        Task<CartClient?> GetByCartIdAsync(Guid id);

        /// <summary>
        /// Asynchronously retrieves a cart client by their client identifier.
        /// </summary>
        /// <param name="clientId">The identifier of the client.</param>
        /// <returns>A task representing the asynchronous operation, containing the cart client with the specified client identifier, if found; otherwise, null.</returns>
        Task<CartClient?> GetByClientIdAsync(Guid clientId);
    }
}
