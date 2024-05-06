using Fiap.McTech.Domain.Enums; 

namespace Fiap.McTech.Domain.Entities.Orders
{
	public class Order : EntityBase
	{
		public Order(Guid clientId, DateTime orderDate, decimal totalAmount, OrderStatus status, List<OrderItem> items)
		{
			ClientId = clientId;
			OrderDate = orderDate;
			TotalAmount = totalAmount;
			Status = status;
			Items = items;
		}

		public Guid ClientId { get; private set; }
		public DateTime OrderDate { get; private set; } = DateTime.UtcNow;
		public decimal TotalAmount { get; private set; } = 0;
		public OrderStatus Status { get; private set; } = OrderStatus.None;
		public List<OrderItem> Items { get; private set; } = new List<OrderItem>();

		public override bool IsValid()
		{
			return ClientId != Guid.Empty &&
				   TotalAmount > 0 &&
				   OrderDate != default;
		}
	}
}