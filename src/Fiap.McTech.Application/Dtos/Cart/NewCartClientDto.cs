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
        new public List<Item> Items { get; set; } = new();

        /// <summary>
        /// Represents the input data for a new item in a shopping cart.
        /// </summary>
        public new class Item : CartClientInputDto.Item
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
}
