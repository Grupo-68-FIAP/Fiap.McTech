using Microsoft.AspNetCore.Mvc;

namespace Fiap.McTech.Api.Controllers.Users
{
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
