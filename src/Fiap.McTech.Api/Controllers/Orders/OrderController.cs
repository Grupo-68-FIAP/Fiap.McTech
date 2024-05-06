using Fiap.McTech.Application.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;
using Fiap.McTech.Application.Interfaces;

namespace Fiap.McTech.Api.Controllers.Orders
{
    public class OrderController : Controller
	{
		public readonly IOrderAppService _OrderAppService;

		public OrderController(IOrderAppService OrderAppService)
		{
			_OrderAppService = OrderAppService;
		}

		[HttpGet("Orders")]
		public async Task<OrderOutputDto> GetOrders()
		{
			return new OrderOutputDto();
		}
	}
}
