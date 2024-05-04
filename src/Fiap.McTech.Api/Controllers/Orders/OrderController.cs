using Fiap.McTech.Application.ViewModels.Orders;
using Fiap.McTech.Application.Interfaces.AppServices;
using Microsoft.AspNetCore.Mvc;

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
		public async Task<OrderOutputViewModel> GetOrders()
		{
			return new OrderOutputViewModel();
		}
	}
}
