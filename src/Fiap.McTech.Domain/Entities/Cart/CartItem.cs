using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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

		public string Name { get; private set; }
		public int Quantity { get; private set; }
		public decimal Value { get; private set; }
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
	}
}
