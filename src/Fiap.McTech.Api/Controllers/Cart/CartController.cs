using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Fiap.McTech.Api.Controllers.Cart
{
    /// <summary>
    /// Controller for managing shopping carts.
    /// </summary>
    [ApiController]
    [Route("api/cart")]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    [Obsolete]
    public class CartController : Controller
    {
        private readonly ICartAppService _cartAppService;
        private readonly IClientAppService _clientAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class.
        /// </summary>
        /// <param name="cartAppService">The shopping cart application service.</param>
        /// <param name="clientAppService">The client application service.</param>
        public CartController(ICartAppService cartAppService, IClientAppService clientAppService)
        {
            _cartAppService = cartAppService;
            _clientAppService = clientAppService;
        }

        /// <summary>
        /// Retrieves a shopping cart by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the shopping cart.</param>
        /// <returns>The requested shopping cart.</returns>
        /// <response code="200">Returns the specified shopping cart.</response>
        /// <response code="404">If the shopping cart with the given ID is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<CartClientOutputDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetCart(Guid id)
        {
            return Ok(await _cartAppService.GetCartByIdAsync(id));
        }

        /// <summary>
        /// Retrieves a shopping cart by the unique identifier of its associated client.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client.</param>
        /// <returns>The shopping cart associated with the client.</returns>
        /// <response code="200">Returns the specified shopping cart.</response>
        /// <response code="204">If the shopping cart for the given client is not found.</response>
        [HttpGet("client/{clientId}")]
        [ProducesResponseType(typeof(List<CartClientOutputDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AllowAnonymous]
        public async Task<IActionResult> GetCartByClientId(Guid clientId)
        {
            return Ok(await _cartAppService.GetCartByClientIdAsync(clientId));
        }

        /// <summary>
        /// Creates a new shopping cart for a client.
        /// </summary>
        /// <param name="cartClientDto">The data of the shopping cart to be created.</param>
        /// <param name="authorization">The authorization token.</param>
        /// <returns>The created shopping cart.</returns>
        /// <response code="201">Returns the newly created shopping cart.</response>
        /// <response code="400">If there are issues with the input data.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CartClientOutputDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<CartClientOutputDto>> CreateCart(CartClientInputDto cartClientDto, [FromHeader] string? authorization)
        {
            var IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;

            if (!string.IsNullOrEmpty(authorization) && !IsAuthenticated)
            {
                return Unauthorized(new { status = 401, detail = "Invalid token." });
            }
            else if (IsAuthenticated)
            {
                var preferredUsername = User?.FindFirst("preferred_username")?.Value ?? "";

                var client = await _clientAppService.GetClientByCpfAsync(preferredUsername);

                cartClientDto.ClientId = client?.Id;
            }

            var createdCart = await _cartAppService.CreateCartClientAsync(cartClientDto);
            return CreatedAtAction(nameof(GetCart), new { id = createdCart.Id }, createdCart);
        }

        /// <summary>
        /// Adds a product to the shopping cart.
        /// </summary>
        /// <param name="id">The unique identifier of the shopping cart.</param>
        /// <param name="productId">The unique identifier of the product to add.</param>
        /// <param name="quantity">The quantity of the product to add.</param>
        /// <returns>The updated shopping cart.</returns>
        /// <response code="200">Returns the updated shopping cart.</response>
        /// <response code="400">If there are validation issues with the input data.</response>
        /// <response code="404">If the shopping cart or product is not found.</response>
        [HttpPut("{id}/product/{productId}")]
        [ProducesResponseType(typeof(CartClientOutputDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> AddCartItemToCartClientAsync(Guid id, Guid productId, [FromQuery] int quantity = 1)
        {
            return Ok(await _cartAppService.AddCartItemToCartClientAsync(id, productId, quantity));
        }

        /// <summary>
        /// Removes an item from the shopping cart.
        /// </summary>
        /// <param name="id">The unique identifier of the shopping cart.</param>
        /// <param name="productId">The unique identifier of the product to remove.</param>
        /// <returns>The updated shopping cart.</returns>
        /// <response code="200">Returns the updated shopping cart after the item has been removed.</response>
        /// <response code="404">If the cart item with the given ID is not found.</response>
        [HttpDelete("{id}/product/{productId}")]
        [ProducesResponseType(typeof(CartClientOutputDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveCartItemFromCartClientAsync(Guid id, Guid productId)
        {
            return Ok(await _cartAppService.RemoveCartItemFromCartClientAsync(id, productId));
        }

        /// <summary>
        /// Deletes an existing shopping cart.
        /// </summary>
        /// <param name="id">The unique identifier of the shopping cart to be deleted.</param>
        /// <returns>A response indicating success or failure.</returns>
        /// <response code="204">Indicates successful deletion of the cart.</response>
        /// <response code="404">If the cart with the given ID is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            await _cartAppService.DeleteCartClientAsync(id);
            return NoContent();
        }
    }
}
