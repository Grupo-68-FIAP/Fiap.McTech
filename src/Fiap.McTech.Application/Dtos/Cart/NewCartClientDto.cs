using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.McTech.Application.Dtos.Cart
{
	public class NewCartClientDto : CartClientInputDto
    {
		public decimal AllValue { get; set; } = 0;

		new public List<NewCartItemDto> Items { get; set; } = new List<NewCartItemDto>();
    }
}