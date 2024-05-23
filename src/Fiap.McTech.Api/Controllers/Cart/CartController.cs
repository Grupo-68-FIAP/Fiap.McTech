using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Exceptions;
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
    public class CartController : Controller
    {
        private readonly ICartAppService _cartAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class.
        /// </summary>
        /// <param name="cartAppService">The shopping cart application service.</param>
        public CartController(ICartAppService cartAppService)
        {
            _cartAppService = cartAppService;
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
        /// Retrieves a shopping cart by the unique identifier of its associated client.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client.</param>
        /// <returns>The shopping cart associated with the client.</returns>
        /// <response code="200">Returns the specified shopping cart.</response>
        /// <response code="204">If the shopping cart for the given client is not found.</response>
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

        /// <summary>
        /// Creates a new shopping cart for a client.
        /// </summary>
        /// <param name="cartClientDto">The data of the shopping cart to be created.</param>
        /// <returns>The created shopping cart.</returns>
        /// <response code="201">Returns the newly created shopping cart.</response>
        /// <response code="400">If there are issues with the input data.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CartClientOutputDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Adds a product to the shopping cart.
        /// </summary>
        /// <param name="id">The unique identifier of the shopping cart.</param>
        /// <param name="productId">The unique identifier of the product to add.</param>
        /// <returns>The updated shopping cart.</returns>
        /// <response code="200">Returns the updated shopping cart.</response>
        /// <response code="404">If the shopping cart or product is not found.</response>
        [HttpPut("{id}/product/{productId}")]
        [ProducesResponseType(typeof(CartClientOutputDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddCartItemToCartClientAsync(Guid id, Guid productId)
        {
            try
            {
                return Ok(await _cartAppService.AddCartItemToCartClientAsync(id, productId));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }
        }

        /// <summary>
        /// Removes an item from the shopping cart.
        /// </summary>
        /// <param name="cartItemId">The unique identifier of the cart item to be removed.</param>
        /// <returns>The updated shopping cart.</returns>
        /// <response code="200">Returns the updated shopping cart after the item has been removed.</response>
        /// <response code="404">If the cart item with the given ID is not found.</response>
        [HttpDelete("product/{cartItemId}")]
        [ProducesResponseType(typeof(CartClientOutputDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveCartItemFromCartClientAsync(Guid cartItemId)
        {
            try
            {
                return Ok(await _cartAppService.RemoveCartItemFromCartClientAsync(cartItemId));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }
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
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            try
            {
                var deleted = await _cartAppService.DeleteCartClientAsync(id);

                if (!deleted.IsSuccess)
                {
                    return BadRequest(deleted.Message);
                }
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }

            return NoContent();
        }
    }
}