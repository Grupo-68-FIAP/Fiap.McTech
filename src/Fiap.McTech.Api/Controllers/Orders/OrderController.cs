using Fiap.McTech.Application.Dtos.Clients;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.ViewModels.Orders;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Fiap.McTech.Api.Controllers.Orders
{
    [Route("api/order")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class OrderController : ControllerBase
    {
        private readonly IOrderAppService _orderAppService;

        public OrderController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        /// <summary>
        /// Obtain order by id
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>The Order</returns>
        /// <response code="200">Returns item</response>
        /// <response code="404">If order isn't exists</response>
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
        /// Change the field status of order
        /// </summary>
        /// <param name="id">Order uuid</param>
        /// <returns>Updated Order</returns>
        /// <response code="200">Returns Order</response>
        /// <response code="402">Wating payment</response>
        /// <response code="404">If order isn't exists</response>
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
        /// Remove an existing order
        /// </summary>
        /// <param name=")">Input order id</param>
        /// <response code="204">Success order removed</response>
        /// <response code="404">If order isn't exists</response>
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
        /// Create a new order based on cart by guid
        /// </summary>
        /// <param name="cartId">Input data of cartId</param>
        /// <returns>Return client</returns>
        /// <response code="201">Return new order</response>
        /// <response code="400">If there validations issues</response>
        /// <response code="404">If cart isn't exists</response>
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
        /// Obtain a list of order by status
        /// </summary>
        /// <param name="status">Status to filter</param>
        /// <returns>Orders list</returns>
        /// <response code="200">Returns all items</response>
        /// <response code="204">If there are nothing</response>
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