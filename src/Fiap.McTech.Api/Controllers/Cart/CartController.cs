using Fiap.McTech.Application.Dtos.Cart;
using Microsoft.AspNetCore.Mvc;
using Fiap.McTech.Application.Interfaces;

namespace Fiap.McTech.Api.Controllers.Cart
{

	[ApiController]
	[Route("api/[controller]")]
    public class CartController : Controller
    {
        public readonly ICartAppService _cartAppService;

		public CartController(ICartAppService cartAppService)
		{
			_cartAppService = cartAppService;
		}

		[HttpGet("{clientId}")]
		public async Task<ActionResult<CartClientOutputDto>> GetCart(Guid clientId)
		{
			return Ok(await _cartAppService.GetCartByClientIdAsync(clientId));
		}

		[HttpPost]
		public async Task<ActionResult<CartClientOutputDto>> CreateCart(CartClientOutputDto cartClientDto)
		{
			var createdCart = await _cartAppService.CreateCartClientAsync(cartClientDto);
			if (createdCart == null)
			{
				return BadRequest("Unable to create cart.");
			}

			return CreatedAtAction(nameof(cartClientDto), new { id = createdCart.ClientId }, createdCart);
		}

		[HttpPut("{clientId}")]
		public async Task<IActionResult> UpdateCart(Guid id, CartClientOutputDto cartClientDto)
		{
			var updatedCart = await _cartAppService.UpdateCartClientAsync(id, cartClientDto);
			if (updatedCart == null)
			{
				return NotFound("Cart not found.");
			}

			return Ok();
		}

		[HttpDelete("{clientId}")]
		public async Task<IActionResult> DeleteCart(Guid id)
		{
			return Ok(await _cartAppService.DeleteCartClientAsync(id));
		}
	}
}