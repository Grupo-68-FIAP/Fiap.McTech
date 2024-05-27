using System;

namespace Fiap.McTech.Application.Dtos.Orders
{
    /// <summary>
    /// Represents the output data for an item in an order.
    /// </summary>
	public class OrderItemOutputDto
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
