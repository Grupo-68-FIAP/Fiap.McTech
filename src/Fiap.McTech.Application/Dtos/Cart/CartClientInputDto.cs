using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.McTech.Application.Dtos.Cart
{
	public class CartClientInputDto
	{
		public Guid? ClientId { get; set; }
		[MinLength(1)]
		public List<CartItemInputDto> Items { get; set; } = new List<CartItemInputDto>();
	}
}