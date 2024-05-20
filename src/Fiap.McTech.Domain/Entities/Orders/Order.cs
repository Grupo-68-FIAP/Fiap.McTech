using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Enums; 

namespace Fiap.McTech.Domain.Entities.Orders
{
	public class Order : EntityBase
	{
		public Order()
		{
		}

		public Guid? ClientId { get; private set; }
		public Client? Client { get; private set; }
        public decimal TotalAmount { get; private set; } = 0;
		public OrderStatus Status { get; private set; } = OrderStatus.None;
		public List<OrderItem> Items { get; private set; } = new List<OrderItem>();

		public override bool IsValid()
		{
			return TotalAmount > 0;
		}
	}
}