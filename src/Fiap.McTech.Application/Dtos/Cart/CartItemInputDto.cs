using System;

namespace Fiap.McTech.Application.Dtos.Cart
{
	public class CartItemInputDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Quantity { get; set; }
		public Guid ProductId { get; set; }
		public Guid CartClientId { get; set; }
	}
}
