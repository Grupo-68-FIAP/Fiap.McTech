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
        public CartClientOutputDto(Guid id, Guid? clientId, decimal allValue, List<Item> items)
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
		public List<Item> Items { get; set; }

        /// <summary>
        /// Represents the output data for a cart item.
        /// </summary>
        public class Item
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CartClientOutputDto.Item"/> class.
            /// </summary>
            /// <param name="id">The ID of the item.</param>
            /// <param name="productId">The ID of the product.</param>
            /// <param name="cartClientId">The ID of the shopping cart client.</param>
            /// <param name="name">The name of the product.</param>
            /// <param name="quantity">The quantity of the product.</param>
            /// <param name="value">The value of the product.</param>
            public Item(Guid id, Guid productId, Guid cartClientId, string name, int quantity, decimal value)
            {
                Id = id;
                ProductId = productId;
                CartClientId = cartClientId;
                Name = name;
                Quantity = quantity;
                Value = value;
            }

            /// <summary>
            /// Gets the ID of the item.
            /// </summary>
            public Guid Id { get; private set; }

            /// <summary>
            /// Gets the ID of the product.
            /// </summary>
            public Guid ProductId { get; private set; }

            /// <summary>
            /// Gets the ID of the shopping cart client.
            /// </summary>
            public Guid CartClientId { get; private set; }

            /// <summary>
            /// Gets the name of the product.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Gets the quantity of the product.
            /// </summary>
            public int Quantity { get; private set; }

            /// <summary>
            /// Gets the value of the product.
            /// </summary>
            public decimal Value { get; private set; }
        }
    }
}
