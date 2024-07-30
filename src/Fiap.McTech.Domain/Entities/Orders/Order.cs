using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Utils.Extensions;

namespace Fiap.McTech.Domain.Entities.Orders
{
    /// <summary>
    /// Represents an order in the system.
    /// </summary>
    public partial class Order : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class with the specified parameters.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client associated with the order.</param>
        /// <param name="totalAmount">The total amount of the order.</param>
        public Order(Guid? clientId, decimal totalAmount)
        {
            ClientId = clientId;
            TotalAmount = totalAmount;
            Status = OrderStatus.None;
            Items = new List<Item>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class with the specified client and total amount.
        /// </summary>
        /// <param name="client">The client associated with the order.</param>
        /// <param name="totalAmount">The total amount of the order.</param>
        public Order(Client client, decimal totalAmount)
        {
            ClientId = client?.Id;
            Client = client;
            TotalAmount = totalAmount;
            Status = OrderStatus.None;
            Items = new List<Item>();
        }

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
        public OrderStatus Status { get; private set; }

        /// <summary>
        /// Gets the list of items in the order.
        /// </summary>
        public ICollection<Item> Items { get; private set; }

        /// <summary>
        /// Determines whether the order is valid.
        /// </summary>
        /// <returns>True if the order is valid; otherwise, false.</returns>
        public override bool IsValid()
        {
            return TotalAmount > 0;
        }

        /// <summary>
        /// Sets the status of the order to the next status.
        /// </summary>
        public void SendToNextStatus()
        {
            Status = Status.NextStatus();
        }

        /// <summary>
        /// Sets the status of the order to canceled.
        /// </summary>
        public void Cancel()
        {
            Status = OrderStatus.Canceled;
        }
    }
}
