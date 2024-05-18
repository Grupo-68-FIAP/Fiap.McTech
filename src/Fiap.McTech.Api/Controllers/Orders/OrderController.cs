using Fiap.McTech.Application.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Application.Dtos.Orders.Add;
using Fiap.McTech.Application.Dtos.Orders.Update;

namespace Fiap.McTech.Api.Controllers.Orders
{
	[Route("api/[controller]")]
	[ApiController]
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
	}
}