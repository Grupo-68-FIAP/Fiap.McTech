using Microsoft.AspNetCore.Mvc;

namespace Fiap.McTech.Api.Controllers.Orders
{
	public class OrderController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
