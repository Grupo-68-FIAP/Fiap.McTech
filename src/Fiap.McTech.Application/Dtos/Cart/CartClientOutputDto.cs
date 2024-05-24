namespace Fiap.McTech.Application.Dtos.Cart
{
    /// <summary>
    /// Represents the output data for a client's shopping cart.
    /// </summary>
    public class CartClientOutputDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartClientOutputDto"/> class.
        /// </summary>
        /// <param name="id">The ID of the cart.</param>
        /// <param name="clientId">The ID of the client.</param>
        /// <param name="allValue">The total value of the cart.</param>
        /// <param name="items">The items in the cart.</param>
        public CartClientOutputDto(Guid id, Guid? clientId, decimal allValue, List<CartItemOutputDto> items)
        {
            Id = id;
            ClientId = clientId;
            AllValue = allValue;
            Items = items;
        }

        /// <summary>
        /// Gets or sets the ID of the cart.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the client.
        /// </summary>
		public Guid? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the total value of the cart.
        /// </summary>
		public decimal AllValue { get; set; }

        /// <summary>
        /// Gets or sets the items in the cart.
        /// </summary>
		public List<CartItemOutputDto> Items { get; set; } = new List<CartItemOutputDto>();
    }
}