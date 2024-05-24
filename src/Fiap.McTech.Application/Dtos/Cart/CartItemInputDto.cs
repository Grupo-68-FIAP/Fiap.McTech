using System.ComponentModel.DataAnnotations;

namespace Fiap.McTech.Application.Dtos.Cart
{
    /// <summary>
    /// Represents the input data for an item in a shopping cart.
    /// </summary>
    public class CartItemInputDto
    {
        /// <summary>
        /// Gets or sets the ID of the product.
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product.
        /// </summary>
        /// <remarks>
        /// Must be a positive integer.
        /// </remarks>
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;
    }
}
