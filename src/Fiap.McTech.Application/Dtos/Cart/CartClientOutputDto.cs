using System;

namespace Fiap.McTech.Application.Dtos.Cart
{
	public class CartClientOutputDto
	{
	   public Guid ClientId { get; set; }

	   public decimal AllValue { get; set; }

	   public List<CartItemOutputDto> Items { get; set; } = new List<CartItemOutputDto>();

	   public CartClientOutputDto(Guid clientId)
		{
			ClientId = clientId;
			AllValue = 0;
		}
	}
}