using Microsoft.AspNetCore.Mvc;

namespace Fiap.McTech.Api.Controllers.Payment
{
	public class PaymentController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
