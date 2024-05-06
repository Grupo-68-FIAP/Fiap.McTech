using System;

namespace Fiap.McTech.Domain.Entities.Cart
{
	public class CartItem : EntityBase
	{
		public CartItem(string name, int quantity, decimal value, Guid productId, Guid cartId, CartClient cartClient)
		{
			Name = name;
			Quantity = quantity;
			Value = value;
			ProductId = productId;
			CartId = cartId;
			CartClient = cartClient;
		}

		public string Name { get; private set; } = string.Empty;
		public int Quantity { get; private set; } = 0;
		public decimal Value { get; private set; } = 0;
		public Guid ProductId { get; private set; }
		public Guid CartId { get; private set; }
		public CartClient CartClient { get; private set; }

		internal decimal CalculateValue()
		{
			return Quantity * Value;
		}

		internal void AddUnities(int unities)
		{
			Quantity += unities;
		}

		internal void UpdateUnities(int unities)
		{
			Quantity = unities;
		}

		public override bool IsValid()
		{
			return !string.IsNullOrWhiteSpace(Name) &&
				   Quantity > 0 &&
				   Value > 0 &&
				   ProductId != Guid.Empty &&
				   CartId != Guid.Empty &&
				   CartClient != null;
		}
	}
}