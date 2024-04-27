using Microsoft.AspNetCore.Mvc;

namespace Fiap.McTech.Api.Controllers.Catalog
{
	public class CatalogController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
