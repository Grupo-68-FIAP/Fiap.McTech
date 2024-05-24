namespace Fiap.McTech.Application.Dtos.Orders.Add
{
    /// <summary>
    /// Represents the input data for creating a new order.
    /// </summary>
	public class CreateOrderInputDto
    {
        /// <summary>
        /// Gets or sets the ID of the client associated with the order.
        /// </summary>
		public Guid? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the items to be added to the order.
        /// </summary>
		public List<CreateOrderItemInputDto> Items { get; set; } = new List<CreateOrderItemInputDto>();
    }

    /// <summary>
    /// Represents the input data for creating a new order item.
    /// </summary>
	public class CreateOrderItemInputDto
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
