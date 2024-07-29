using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Application.ViewModels.Orders
{
    /// <summary>
    /// Represents the output data for an order.
    /// </summary>
	public class OrderOutputDto
    {
        /// <summary>
        /// Gets or sets the ID of the order.
        /// </summary>
		public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the client associated with the order.
        /// </summary>
		public Guid? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the name of the client associated with the order.
        /// </summary>
		public string? ClientName { get; set; }

        /// <summary>
        /// Gets or sets the total amount of the order.
        /// </summary>
		public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the status of the order.
        /// </summary>
		public OrderStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the items included in the order.
        /// </summary>
		public List<Item> Items { get; set; } = new();

        /// <summary>
        /// Represents the output data for an item in an order.
        /// </summary>
        public class Item
        {
            /// <summary>
            /// Gets or sets the ID of the product.
            /// </summary>
            public Guid ProductId { get; set; }

            /// <summary>
            /// Gets or sets the ID of the order.
            /// </summary>
            public Guid OrderId { get; set; }

            /// <summary>
            /// Gets or sets the name of the product.
            /// </summary>
            public string ProductName { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the price of the product.
            /// </summary>
            public decimal Price { get; set; }

            /// <summary>
            /// Gets or sets the quantity of the product.
            /// </summary>
            public int Quantity { get; set; }

            /// <summary>
            /// Gets or sets the total price of the item.
            /// </summary>
            public decimal TotalPrice { get; set; }
        }
    }
}
