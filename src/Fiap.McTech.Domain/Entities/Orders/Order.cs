using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Domain.Entities.Orders
{
    /// <summary>
    /// Represents an order in the system.
    /// </summary>
    public class Order : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        public Order() { }

        /// <summary>
        /// Gets or sets the unique identifier of the client associated with the order.
        /// </summary>
        public Guid? ClientId { get; private set; }

        /// <summary>
        /// Gets or sets the client associated with the order.
        /// </summary>
        public Client? Client { get; private set; }

        /// <summary>
        /// Gets or sets the total amount of the order.
        /// </summary>
        public decimal TotalAmount { get; private set; } = 0;

        /// <summary>
        /// Gets or sets the status of the order.
        /// </summary>
        public OrderStatus Status { get; private set; } = OrderStatus.None;

        /// <summary>
        /// Gets the list of items in the order.
        /// </summary>
        public List<OrderItem> Items { get; private set; } = new List<OrderItem>();

        /// <summary>
        /// Determines whether the order is valid.
        /// </summary>
        /// <returns>True if the order is valid; otherwise, false.</returns>
        public override bool IsValid()
        {
            return TotalAmount > 0;
        }
    }
}
