using Fiap.McTech.Application.Dtos.Orders;
using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Application.ViewModels.Orders
{
	public class OrderOutputDto
	{
		public Guid Id { get; set; }
		public Guid? ClientId { get; set; }
		public string? ClientName { get; set; }
		public decimal TotalAmount { get; set; }
		public OrderStatus Status { get; set; }
		public List<OrderItemOutputDto> Items { get; set; } = new List<OrderItemOutputDto>();
	}
}