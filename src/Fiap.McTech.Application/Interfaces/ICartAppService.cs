using Fiap.McTech.Application.Dtos.Cart;

namespace Fiap.McTech.Application.Interfaces
{
    /// <summary>
    /// Represents the application service interface for managing shopping carts.
    /// </summary>
    public interface ICartAppService
    {
        /// <summary>
        /// Retrieves a shopping cart by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the shopping cart to retrieve.</param>
        /// <returns>A <see cref="CartClientOutputDto"/> representing the retrieved shopping cart, or null if not found.</returns>
        Task<CartClientOutputDto?> GetCartByIdAsync(Guid id);

        /// <summary>
        /// Retrieves a shopping cart by its client ID asynchronously.
        /// </summary>
        /// <param name="clientId">The ID of the client associated with the shopping cart.</param>
        /// <returns>A <see cref="CartClientOutputDto"/> representing the retrieved shopping cart, or null if not found.</returns>
        Task<CartClientOutputDto?> GetCartByClientIdAsync(Guid clientId);

        /// <summary>
        /// Creates a new shopping cart for a client asynchronously.
        /// </summary>
        /// <param name="cart">The <see cref="CartClientInputDto"/> containing the details of the new shopping cart.</param>
        /// <returns>A <see cref="CartClientOutputDto"/> representing the created shopping cart.</returns>
        Task<CartClientOutputDto> CreateCartClientAsync(CartClientInputDto cart);

        /// <summary>
        /// Adds a cart item to the specified shopping cart asynchronously.
        /// </summary>
        /// <param name="cartId">The ID of the shopping cart to add the item to.</param>
        /// <param name="productId">The ID of the product to add to the shopping cart.</param>
        /// <param name="quantities">The number of items to add to the shopping cart.</param>
        /// <returns>A <see cref="CartClientOutputDto"/> representing the updated shopping cart.</returns>
        Task<CartClientOutputDto> AddCartItemToCartClientAsync(Guid cartId, Guid productId, int quantities = 1);

        /// <summary>
        /// Removes a cart item from the specified shopping cart asynchronously.
        /// </summary>
        /// <param name="cartId">The ID of the shopping cart to remove the item from.</param>
        /// <param name="productId">The ID of the product to remove from the shopping cart.</param>
        /// <returns>A <see cref="CartClientOutputDto"/> representing the updated shopping cart.</returns>
        Task<CartClientOutputDto> RemoveCartItemFromCartClientAsync(Guid cartId, Guid productId);

        /// <summary>
        /// Deletes a shopping cart for a client asynchronously.
        /// </summary>
        /// <param name="id">The ID of the client associated with the shopping cart to delete.</param>
        Task DeleteCartClientAsync(Guid id);
    }
}
