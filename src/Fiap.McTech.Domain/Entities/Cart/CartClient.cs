using Fiap.McTech.Domain.Entities.Clients;
using System;

namespace Fiap.McTech.Domain.Entities.Cart
{
	public class CartClient : EntityBase
	{
		public CartClient() 
		{
		}

		public Guid? ClientId { get; internal set; }
		public Client? Client { get; internal set; }
        public decimal AllValue { get; internal set; } = 0;
		public List<CartItem> Items { get; internal set; } = new List<CartItem>();

		public override bool IsValid()
		{
			return Id != Guid.Empty 
				&& Items != null && Items.Count > 0 && Items.All(i => i.IsValid());
		}

		public void CalculateValueCart()
		{
			AllValue = Items.Sum(p => p.CalculateValue());
		}
	}
}