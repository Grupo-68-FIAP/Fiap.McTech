using Fiap.McTech.Domain.Entities.Cart;

namespace Fiap.McTech.Application.Dtos.Cart
{
    /// <summary>
    /// Represents the input data for a new item in a shopping cart.
    /// </summary>
    public class NewCartItemDto : CartItemInputDto
    {
        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value of the product.
        /// </summary>
        public decimal Value { get; set; } = decimal.Zero;
    }
}
