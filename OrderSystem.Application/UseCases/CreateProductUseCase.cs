namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Interfaces;

public class CreateProductUseCase
{
    private readonly IProductRepository _repository;

    public CreateProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductResponseDto> Execute(CreateProductDto dto)
    {
        var product = new Product(dto.Name, dto.Price);
        await _repository.Add(product);

        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
}
