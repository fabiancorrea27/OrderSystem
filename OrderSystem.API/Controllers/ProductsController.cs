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

    public ProductsController(
        GetProductsUseCase getProductsUseCase,
        CreateProductUseCase createProductUseCase)
    {
        _getProductsUseCase = getProductsUseCase;
        _createProductUseCase = createProductUseCase;
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
}
