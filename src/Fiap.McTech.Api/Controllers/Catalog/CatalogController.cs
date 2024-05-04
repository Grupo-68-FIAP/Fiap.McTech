using Fiap.McTech.Application.ViewModels.Catalog;
using Fiap.McTech.Application.Interfaces.AppServices;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.McTech.Api.Controllers.Catalog
{
	public class CatalogController : Controller
	{
		public readonly ICatalogAppService _CatalogAppService;

		public CatalogController(ICatalogAppService CatalogAppService)
		{
			_CatalogAppService = CatalogAppService;
		}

		[HttpGet("Catalog")]
		public async Task<CatalogOutputViewModel> GetCatalog()
		{
			return new CatalogOutputViewModel();
		}
	}
}
