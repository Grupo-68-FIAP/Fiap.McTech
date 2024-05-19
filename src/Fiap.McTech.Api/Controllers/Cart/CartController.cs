using Fiap.McTech.Application.Dtos.Cart;
using Microsoft.AspNetCore.Mvc;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Exceptions;

namespace Fiap.McTech.Api.Controllers.Cart
{

	[ApiController]
	[Route("api/cart")]
    public class CartController : Controller
    {
        public readonly ICartAppService _cartAppService;

		public CartController(ICartAppService cartAppService)
		{
			_cartAppService = cartAppService;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CartClientOutputDto>> GetCart(Guid id)
		{
			return Ok(await _cartAppService.GetCartByIdAsync(id));
		}

		[HttpPost]
		public async Task<ActionResult<CartClientOutputDto>> CreateCart(CartClientInputDto cartClientDto)
		{
			CartClientOutputDto createdCart;
			try
			{
				createdCart = await _cartAppService.CreateCartClientAsync(cartClientDto);
			}
			catch(Exception ex)
			{
				if (ex is EntityNotFoundException)
				{
					return NotFound("Client not found.");
				}
				else 
				{
					throw;
				}
			}

			if (createdCart == null)
			{
				return BadRequest("Unable to create cart.");
			}

			return CreatedAtAction(nameof(GetCart), new { id = createdCart.Id }, createdCart);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCart(Guid id, CartClientOutputDto cartClientDto)
		{
			var updatedCart = await _cartAppService.UpdateCartClientAsync(id, cartClientDto);
			if (updatedCart == null)
			{
				return NotFound("Cart not found.");
			}

			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCart(Guid id)
		{
			var deleted = await _cartAppService.DeleteCartClientAsync(id);
			
			if (!deleted.IsSuccess) {
				return BadRequest(deleted.Message);
			}

			return NoContent();
		}
	}
}