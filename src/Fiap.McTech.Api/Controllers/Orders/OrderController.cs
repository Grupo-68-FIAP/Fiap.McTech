using Fiap.McTech.Application.Dtos.Clients;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.ViewModels.Orders;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Fiap.McTech.Api.Controllers.Orders
{
    /// <summary>
    /// Controller responsible for handling operations related to orders.
    /// </summary>
    [Route("api/order")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class OrderController : ControllerBase
    {
        private readonly IOrderAppService _orderAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class with the specified order application service.
        /// </summary>
        /// <param name="orderAppService">The service to manage order operations.</param>
        public OrderController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        /// <summary>
        /// Retrieves an order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <returns>The requested order.</returns>
        /// <response code="200">Returns the order corresponding to the provided ID.</response>
        /// <response code="404">Indicates that the order with the specified ID does not exist.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderOutputDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            try
            {
                return Ok(await _orderAppService.GetOrderByIdAsync(id));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }
        }

        /// <summary>
        /// Advances the status of an order to the next step in the workflow.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <returns>The updated order with its new status.</returns>
        /// <response code="200">Returns the order with its updated status.</response>
        /// <response code="402">Indicates that the order cannot proceed due to a pending payment.</response>
        /// <response code="404">Indicates that the order with the specified ID does not exist.</response>
        [HttpPut("{id}/nextstatus")]
        [ProducesResponseType(typeof(OrderOutputDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status402PaymentRequired)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MoveOrderToNextStatus(Guid id)
        {
            try
            {
                return Ok(await _orderAppService.MoveOrderToNextStatus(id));
            }
            catch (PaymentRequiredException ex)
            {
                return StatusCode(402, new ProblemDetails() { Detail = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }
        }

        /// <summary>
        /// Deletes an order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the order to be deleted.</param>
        /// <response code="204">Indicates that the order was successfully deleted.</response>
        /// <response code="404">Indicates that the order with the specified ID does not exist.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var result = await _orderAppService.DeleteOrderAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new order based on the specified cart ID.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart to create the order from.</param>
        /// <returns>The newly created order.</returns>
        /// <response code="201">Returns the newly created order.</response>
        /// <response code="400">Indicates that there were validation issues with the provided data.</response>
        /// <response code="404">Indicates that the cart with the specified ID does not exist.</response>
        [HttpPost("{cartId}")]
        [ProducesResponseType(typeof(OrderOutputDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateOrder(Guid cartId)
        {
            try
            {
                var createdOrder = await _orderAppService.CreateOrderByCartAsync(cartId);
                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ProblemDetails() { Detail = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a list of orders filtered by their status.
        /// </summary>
        /// <param name="status">
        /// Status to filter orders. Possible values are:
        /// <list type="bullet">
        /// <item><description><c>None</c> (-1): No specific status.</description></item>
        /// <item><description><c>Pending</c> (0): Orders that are pending.</description></item>
        /// <item><description><c>Processing</c> (1): Orders that are being processed.</description></item>
        /// <item><description><c>Completed</c> (2): Orders that are completed.</description></item>
        /// <item><description><c>Canceled</c> (3): Orders that are canceled.</description></item>
        /// </list>
        /// </param>
        /// <returns>A list of orders matching the specified status.</returns>
        /// <response code="200">Returns the list of orders with the specified status.</response>
        /// <response code="204">Indicates that no orders with the specified status were found.</response>
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(List<ClientOutputDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetOrderByStatus(OrderStatus status)
        {
            var orders = await _orderAppService.GetOrderByStatusAsync(status);
            return (orders == null || !orders.Any()) ? new NoContentResult() : Ok(orders);
        }
    }
}