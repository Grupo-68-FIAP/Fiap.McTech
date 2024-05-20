using System;

namespace Fiap.McTech.Application.Dtos.Orders
{
	public class OrderItemOutputDto
	{
		public Guid ProductId { get; set; }
		public Guid OrderId { get; set; }
		public string ProductName { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public decimal TotalPrice { get; set; }
	}
}