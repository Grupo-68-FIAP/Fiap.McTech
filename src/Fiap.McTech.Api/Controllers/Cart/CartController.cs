using Fiap.McTech.Application.ViewModels.Cart;
using Fiap.McTech.Domain.Entities.Cart;
using Microsoft.AspNetCore.Mvc;
using Fiap.McTech.Application.Interfaces;

namespace Fiap.McTech.Api.Controllers.Cart
{
    public class CartController : Controller
    {
        public readonly ICartAppService _cartAppService;

		public CartController(ICartAppService cartAppService)
		{
			_cartAppService = cartAppService;
		}

		[HttpGet("cart")]
		public async Task<CartOutputViewModel> GetCart()
		{
			return new CartOutputViewModel();
		}
	}
}