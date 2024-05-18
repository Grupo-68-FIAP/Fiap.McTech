using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Application.Dtos.Orders.Update
{
	public class UpdateOrderInputDto
	{
		public Guid? ClientId { get; set; }
		public OrderStatus Status { get; set; }
		public List<UpdateOrderItemInputDto> Items { get; set; } = new List<UpdateOrderItemInputDto>();
	}

	public class UpdateOrderItemInputDto
	{
		public Guid ProductId { get; set; }
		public string ProductName { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}