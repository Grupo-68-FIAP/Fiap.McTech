using System;

namespace Fiap.McTech.Application.Dtos.Cart
{
	public class CartClientOutputDto
	{
		public Guid Id { get; set; }
		public Guid ClientId { get; set; }
		public decimal AllValue { get; set; }
		public List<CartItemOutputDto> Items { get; set; } = new List<CartItemOutputDto>();
	}
}