namespace Fiap.McTech.Application.Dtos.Cart
{
    /// <summary>
    /// Represents the input data for creating a new client's shopping cart.
    /// </summary>
	public class NewCartClientDto : CartClientInputDto
    {
        /// <summary>
        /// Gets or sets the total value of the cart.
        /// </summary>
		public decimal AllValue { get; set; } = 0;

        /// <summary>
        /// Gets or sets the items in the shopping cart.
        /// </summary>
        /// <remarks>
        /// Overriding the base class property to use the derived type <see cref="NewCartItemDto"/>.
        /// </remarks>
        new public List<NewCartItemDto> Items { get; set; } = new List<NewCartItemDto>();
    }
}
