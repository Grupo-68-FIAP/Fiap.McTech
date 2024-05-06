using System;

namespace Fiap.McTech.Domain.Entities.Orders
{
	public class OrderItem : EntityBase
	{
		public OrderItem(Guid productId, string productName, decimal price, int quantity)
		{
			ProductId = productId;
			ProductName = productName;
			Price = price;
			Quantity = quantity;
		}

		public Guid ProductId { get; private set; }
		public string ProductName { get; private set; } = string.Empty;
		public decimal Price { get; private set; } = 0;
		public int Quantity { get; private set; } = 0;

		public override bool IsValid()
		{
			return ProductId != Guid.Empty &&
				   !string.IsNullOrWhiteSpace(ProductName) &&
				   Price > 0 &&
				   Quantity > 0;
		}
	}
}