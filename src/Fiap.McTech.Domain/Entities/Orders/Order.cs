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
        /// Inicializa uma nova instância da classe <see cref="Order"/>.
        /// </summary>
        /// <param name="clientId">O ID do cliente.</param>
        /// <param name="totalAmount">O valor total do pedido.</param>
        /// <param name="client">O cliente associado ao pedido.</param>
        public Order(Guid? clientId, decimal totalAmount, Client? client = null)
        {
            ClientId = clientId;
            TotalAmount = totalAmount;
            Status = OrderStatus.None;
            Items = new List<Item>();
            Client = client;
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
