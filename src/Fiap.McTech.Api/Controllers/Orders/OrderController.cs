using Fiap.McTech.Application.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.Dtos.Orders.Add;
using Fiap.McTech.Application.Dtos.Orders.Update;
using System.Net.Mime;
using Fiap.McTech.Application.Dtos.Clients;
using Fiap.McTech.Domain.Exceptions;

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

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderOutputDto>> GetOrderById(Guid id)
        {
            var order = await _orderAppService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpPost]
        public async Task<ActionResult<OrderOutputDto>> CreateOrder(CreateOrderInputDto orderDto)
        {
            var createdOrder = await _orderAppService.CreateOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderOutputDto>> UpdateOrder(Guid id, UpdateOrderInputDto orderDto)
        {
            var updatedOrder = await _orderAppService.UpdateOrderAsync(id, orderDto);
            if (updatedOrder == null)
            {
                return NotFound();
            }

            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
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
    }
}