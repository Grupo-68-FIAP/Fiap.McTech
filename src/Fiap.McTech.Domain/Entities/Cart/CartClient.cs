using System;

namespace Fiap.McTech.Domain.Entities.Cart
{
	public class CartClient : EntityBase
	{
		public CartClient(Guid clientId, decimal allValue, List<CartItem> items)
		{
			ClientId = clientId;
			AllValue = allValue;
			Items = items;
		}

		public Guid ClientId { get; private set; }
		public decimal AllValue { get; private set; } = 0;
		public List<CartItem> Items { get; private set; } = new List<CartItem>();

		internal void CalculateValueCart()
		{
			AllValue = Items.Sum(p => p.CalculateValue());
		}
	}
}