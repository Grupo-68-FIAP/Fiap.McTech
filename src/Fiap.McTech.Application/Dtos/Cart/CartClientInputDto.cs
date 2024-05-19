using System;

namespace Fiap.McTech.Application.Dtos.Cart
{
	public class CartClientInputDto
	{
		public Guid Id { get; set; }
		public Guid ClientId { get; set; }
		public decimal AllValue { get; set; }
		public List<CartItemInputDto> Items { get; set; } = new List<CartItemInputDto>();
	}
}