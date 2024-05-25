namespace Fiap.McTech.Domain.Entities.Orders
{
    /// <summary>
    /// Represents an item in an order.
    /// </summary>
    public class OrderItem : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItem"/> class.
        /// </summary>
        public OrderItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItem"/> class with specified parameters.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="orderId">The unique identifier of the order associated with the item.</param>
        /// <param name="productName">The name of the product.</param>
        /// <param name="price">The price of the product.</param>
        /// <param name="quantity">The quantity of the product.</param>
        public OrderItem(Guid productId, Guid orderId, string productName, decimal price, int quantity)
        {
            ProductId = productId;
            OrderId = orderId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the product associated with the item.
        /// </summary>
        public Guid ProductId { get; private set; }

        /// <summary>
        /// Gets or sets the unique identifier of the order associated with the item.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string ProductName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        public decimal Price { get; private set; } = 0;

        /// <summary>
        /// Gets or sets the quantity of the product.
        /// </summary>
        public int Quantity { get; private set; } = 0;

        /// <summary>
        /// Gets or sets the order associated with the item.
        /// </summary>
        public Order? Order { get; private set; }

        /// <summary>
        /// Determines whether the order item is valid.
        /// </summary>
        /// <returns>True if the order item is valid; otherwise, false.</returns>
        public override bool IsValid()
        {
            return ProductId != Guid.Empty &&
                   !string.IsNullOrWhiteSpace(ProductName) &&
                   Price > 0 &&
                   Quantity > 0;
        }
    }
}
