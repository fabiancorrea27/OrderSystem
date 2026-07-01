namespace OrderSystem.API.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystem.Application.DTOs;
using OrderSystem.Application.UseCases;

[ApiController]
[Route("api/cart")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly GetCartUseCase _getCart;
    private readonly AddCartItemUseCase _addItem;
    private readonly UpdateCartItemQtyUseCase _updateQty;
    private readonly RemoveCartItemUseCase _removeItem;
    private readonly ClearCartUseCase _clearCart;
    private readonly MergeCartUseCase _mergeCart;
    private readonly UpdateCartItemPriceUseCase _updatePrice;

    public CartController(
        GetCartUseCase getCart,
        AddCartItemUseCase addItem,
        UpdateCartItemQtyUseCase updateQty,
        RemoveCartItemUseCase removeItem,
        ClearCartUseCase clearCart,
        MergeCartUseCase mergeCart,
        UpdateCartItemPriceUseCase updatePrice)
    {
        _getCart = getCart;
        _addItem = addItem;
        _updateQty = updateQty;
        _removeItem = removeItem;
        _clearCart = clearCart;
        _mergeCart = mergeCart;
        _updatePrice = updatePrice;
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();

        var cart = await _getCart.Execute(userId);
        return cart is null ? Ok(new CartResponseDto()) : Ok(cart);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] CartItemRequestDto dto)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();

        try
        {
            var cart = await _addItem.Execute(dto, userId);
            return Ok(cart);
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

    [HttpPut("items/{productId:guid}")]
    public async Task<IActionResult> UpdateQty(Guid productId, [FromBody] CartItemRequestDto dto)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();

        try
        {
            var cart = await _updateQty.Execute(productId, dto.Quantity, userId);
            return Ok(cart);
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

    [HttpDelete("items/{productId:guid}")]
    public async Task<IActionResult> RemoveItem(Guid productId)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();

        try
        {
            var cart = await _removeItem.Execute(productId, userId);
            return Ok(cart);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPut("items/{productId:guid}/price")]
    public async Task<IActionResult> UpdatePrice(Guid productId, [FromBody] UpdatePriceDto dto)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();

        try
        {
            var cart = await _updatePrice.Execute(productId, dto.Price, userId);
            return Ok(cart);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Clear()
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();

        await _clearCart.Execute(userId);
        return NoContent();
    }

    [HttpPost("merge")]
    public async Task<IActionResult> Merge([FromBody] List<CartItemRequestDto> items)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();

        var cart = await _mergeCart.Execute(items, userId);
        return Ok(cart ?? new CartResponseDto());
    }
}
