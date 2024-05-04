using Microsoft.AspNetCore.Mvc;

namespace Fiap.McTech.Api.Controllers.Clients
{
	public class ClientController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
