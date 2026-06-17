namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Interfaces;

public class UpdateProductStockUseCase
{
    private readonly IProductRepository _productRepository;

    public UpdateProductStockUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponseDto> Execute(Guid productId, UpdateProductStockDto dto)
    {
        var product = await _productRepository.GetById(productId);
        if (product is null)
            throw new KeyNotFoundException($"Product with id {productId} not found.");

        product.SetStock(dto.Stock);
        await _productRepository.Update(product);

        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock
        };
    }
}