using System.ComponentModel.DataAnnotations;

namespace Fiap.McTech.Application.Dtos.Cart
{
    /// <summary>
    /// Represents the input data for a client's shopping cart.
    /// </summary>
    public class CartClientInputDto
    {
        /// <summary>
        /// Gets or sets the client's ID.
        /// </summary>
        public Guid? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the items in the shopping cart.
        /// </summary>
        /// <remarks>
        /// Must have at least one item.
        /// </remarks>
        [MinLength(1)]
        public List<CartItemInputDto> Items { get; set; } = new List<CartItemInputDto>();
    }
}
