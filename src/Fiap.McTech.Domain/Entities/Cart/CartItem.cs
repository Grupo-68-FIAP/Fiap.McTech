using Fiap.McTech.Domain.Entities.Products;

namespace Fiap.McTech.Domain.Entities.Cart
{
    /// <summary>
    /// Represents an item in a shopping cart.
    /// </summary>
    public class CartItem : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartItem"/> class.
        /// </summary>
        public CartItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartItem"/> class with specified parameters.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <param name="quantity">The quantity of the product.</param>
        /// <param name="value">The value of the product.</param>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="cartClientId">The unique identifier of the client's shopping cart.</param>
        public CartItem(string name, int quantity, decimal value, Guid productId, Guid cartClientId)
        {
            Name = name;
            Quantity = quantity;
            Value = value;
            ProductId = productId;
            CartClientId = cartClientId;
        }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string Name { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets the quantity of the product.
        /// </summary>
        public int Quantity { get; internal set; } = 0;

        /// <summary>
        /// Gets or sets the value of the product.
        /// </summary>
        public decimal Value { get; internal set; } = 0;

        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        public Guid ProductId { get; internal set; }

        /// <summary>
        /// Gets or sets the unique identifier of the client's shopping cart.
        /// </summary>
        public Guid CartClientId { get; internal set; }

        /// <summary>
        /// Gets or sets the client's shopping cart.
        /// </summary>
        public CartClient? CartClient { get; internal set; }

        /// <summary>
        /// Gets or sets the product associated with the cart item.
        /// </summary>
        public Product? Product { get; internal set; }

        /// <summary>
        /// Calculates the total value of the cart item.
        /// </summary>
        /// <returns>The total value of the cart item.</returns>
        internal decimal CalculateValue()
        {
            return Quantity * Value;
        }

        /// <summary>
        /// Adds units to the quantity of the cart item.
        /// </summary>
        /// <param name="unities">The number of units to add.</param>
        internal void AddUnities(int unities)
        {
            Quantity += unities;
        }

        /// <summary>
        /// Updates the quantity of the cart item.
        /// </summary>
        /// <param name="unities">The new quantity of the cart item.</param>
        internal void UpdateUnities(int unities)
        {
            Quantity = unities;
        }

        /// <summary>
        /// Determines whether the cart item is valid.
        /// </summary>
        /// <returns>True if the cart item is valid; otherwise, false.</returns>
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   Quantity > 0 &&
                   Value > 0 &&
                   ProductId != Guid.Empty;
        }
    }
}
