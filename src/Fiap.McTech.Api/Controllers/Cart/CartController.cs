using Fiap.McTech.Application.Dtos.Cart;
using Microsoft.AspNetCore.Mvc;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Exceptions;
using System.Net.Mime;
using Fiap.McTech.Application.Dtos.Clients;

namespace Fiap.McTech.Api.Controllers.Cart
{

    [ApiController]
    [Route("api/cart")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CartController : Controller
    {
        public readonly ICartAppService _cartAppService;

        public CartController(ICartAppService cartAppService)
        {
            _cartAppService = cartAppService;
        }

        /// <summary>
        /// Obtain a client cart by uuid.
        /// </summary>
        /// <param name="id">CartClient uuid</param>
        /// <returns>The cart of id</returns>
        /// <response code="200">The cart</response>
        /// <response code="204">If there are nothing</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<CartClientOutputDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetCart(Guid id)
        {
            try
            {
                return Ok(await _cartAppService.GetCartByIdAsync(id));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }
        }

        /// <summary>
        /// Obtain a client cart by client uuid.
        /// </summary>
        /// <param name="clientId">Client uuid</param>
        /// <returns>The cart of id</returns>
        /// <response code="200">The cart</response>
        /// <response code="204">If there are nothing</response>
        [HttpGet("client/{clientId}")]
        [ProducesResponseType(typeof(List<CartClientOutputDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetCartByClientId(Guid clientId)
        {
            try
            {
                return Ok(await _cartAppService.GetCartByClientIdAsync(clientId));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CartClientOutputDto>> CreateCart(CartClientInputDto cartClientDto)
        {
            CartClientOutputDto createdCart;
            try
            {
                createdCart = await _cartAppService.CreateCartClientAsync(cartClientDto);
            }
            catch (McTechException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }
            catch (Exception)
            {
                throw;
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

            if (!deleted.IsSuccess)
            {
                return BadRequest(deleted.Message);
            }

            return NoContent();
        }
    }
}