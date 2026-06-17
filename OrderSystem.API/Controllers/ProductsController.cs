namespace OrderSystem.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystem.Application.DTOs;
using OrderSystem.Application.UseCases;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly GetProductsUseCase _getProductsUseCase;
    private readonly CreateProductUseCase _createProductUseCase;
    private readonly UpdateProductStockUseCase _updateProductStockUseCase;

    public ProductsController(
        GetProductsUseCase getProductsUseCase,
        CreateProductUseCase createProductUseCase,
        UpdateProductStockUseCase updateProductStockUseCase)
    {
        _getProductsUseCase = getProductsUseCase;
        _createProductUseCase = createProductUseCase;
        _updateProductStockUseCase = updateProductStockUseCase;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var products = await _getProductsUseCase.Execute();
        return Ok(products);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var result = await _createProductUseCase.Execute(dto);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpPut("{id}/stock")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStock(Guid id, [FromBody] UpdateProductStockDto dto)
    {
        try
        {
            var result = await _updateProductStockUseCase.Execute(id, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
