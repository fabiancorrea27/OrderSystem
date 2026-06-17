namespace OrderSystem.API.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystem.Application.DTOs;
using OrderSystem.Application.UseCases;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly CreateOrderUseCase _createOrderUseCase;
    private readonly GetMyOrdersUseCase _getMyOrdersUseCase;

    public OrdersController(
        CreateOrderUseCase createOrderUseCase,
        GetMyOrdersUseCase getMyOrdersUseCase)
    {
        _createOrderUseCase = createOrderUseCase;
        _getMyOrdersUseCase = getMyOrdersUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        try
        {
            var result = await _createOrderUseCase.Execute(dto, userId);
            return CreatedAtAction(nameof(GetMy), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMy()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var orders = await _getMyOrdersUseCase.Execute(userId);
        return Ok(orders);
    }
}
