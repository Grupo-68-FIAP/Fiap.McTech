using System;

namespace Fiap.McTech.Domain.Entities.Cart
{
	public class CartClient : EntityBase
	{
		public CartClient(Guid clientId)
		{
			ClientId = clientId;
		}

		public Guid ClientId { get; private set; }
		public decimal AllValue { get; private set; } = 0;
		public List<CartItem> Items { get; private set; } = new List<CartItem>();

		public override bool IsValid()
		{	 
			return ClientId != Guid.Empty && Items != null && Items.Count > 0;
		}

		internal void CalculateValueCart()
		{
			AllValue = Items.Sum(p => p.CalculateValue());
		}
	}
}