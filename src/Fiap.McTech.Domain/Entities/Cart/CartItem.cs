﻿using Fiap.McTech.Domain.Entities.Products;
using System;

namespace Fiap.McTech.Domain.Entities.Cart
{
	public class CartItem : EntityBase
	{
		public CartItem() { }

		public CartItem(string name, int quantity, decimal value, Guid productId, Guid cartClientId)
		{
			Name = name;
			Quantity = quantity;
			Value = value;
			ProductId = productId;
            CartClientId = cartClientId;
		}

		public string Name { get; internal set; } = string.Empty;
		public int Quantity { get; internal set; } = 0;
		public decimal Value { get; internal set; } = 0;
		public Guid ProductId { get; internal set; }
		public Guid CartClientId { get; internal set; }
		public CartClient? CartClient { get; internal set; }
        public Product? Product { get; internal set; }

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
				   CartClientId != Guid.Empty;
		}
	}
}