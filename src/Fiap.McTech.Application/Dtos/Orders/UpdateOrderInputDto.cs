using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Application.Dtos.Orders
{
    /// <summary>
    /// Represents the input data for updating an order.
    /// </summary>
	public class UpdateOrderInputDto
    {
        /// <summary>
        /// Gets or sets the ID of the client associated with the order.
        /// </summary>
		public Guid? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the status of the order.
        /// </summary>
		public OrderStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the items to be updated in the order.
        /// </summary>
		public List<Item> Items { get; set; } = new();

        /// <summary>
        /// Represents the input data for updating an order item.
        /// </summary>
        public class Item
        {
            /// <summary>
            /// Gets or sets the ID of the product.
            /// </summary>
            public Guid ProductId { get; set; }

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
        }
    }
}
