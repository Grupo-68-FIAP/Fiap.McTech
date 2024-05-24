namespace Fiap.McTech.Application.Dtos.Cart
{
    /// <summary>
    /// Represents the output data for an item in a shopping cart.
    /// </summary>
    public class CartItemOutputDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemOutputDto"/> class.
        /// </summary>
        /// <param name="id">The ID of the item.</param>
        /// <param name="productId">The ID of the product.</param>
        /// <param name="cartClientId">The ID of the shopping cart client.</param>
        /// <param name="name">The name of the product.</param>
        /// <param name="quantity">The quantity of the product.</param>
        /// <param name="value">The value of the product.</param>
        public CartItemOutputDto(Guid id, Guid productId, Guid cartClientId, string name, int quantity, decimal value)
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