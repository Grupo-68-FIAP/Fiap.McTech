using Fiap.McTech.Application.Dtos.Orders;
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
		public List<OrderItemOutputDto> Items { get; set; } = new List<OrderItemOutputDto>();
    }
}
