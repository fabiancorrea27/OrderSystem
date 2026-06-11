namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Interfaces;

public class GetProductsUseCase
{
    private readonly IProductRepository _repository;

    public GetProductsUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductResponseDto>> Execute()
    {
        var products = await _repository.GetAll();

        return products.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        }).ToList();
    }
}
