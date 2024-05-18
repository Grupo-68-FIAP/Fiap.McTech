using System;

namespace Fiap.McTech.Application.Dtos.Orders.Add
{
	public class CreateOrderInputDto
	{
		public Guid? ClientId { get; set; }
		public List<CreateOrderItemInputDto> Items { get; set; } = new List<CreateOrderItemInputDto>();
	}

	public class CreateOrderItemInputDto
	{
		public Guid ProductId { get; set; }
		public string ProductName { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}