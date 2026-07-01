namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Interfaces;

public class GetCartUseCase
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public GetCartUseCase(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<CartResponseDto?> Execute(Guid userId)
    {
        var cart = await _cartRepository.GetByUserId(userId);
        if (cart is null) return null;

        return await ToDto(cart);
    }

    public async Task<CartResponseDto> ToDto(Domain.Entities.Cart cart)
    {
        var productIds = cart.Items.Select(i => i.ProductId).ToList();
        var products = await _productRepository.GetByIds(productIds);
        var productMap = products.ToDictionary(p => p.Id);

        return new CartResponseDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.Items.Select(i =>
            {
                productMap.TryGetValue(i.ProductId, out var product);
                return new CartItemResponseDto
                {
                    ProductId = i.ProductId,
                    ProductName = product?.Name ?? "Deleted product",
                    Price = i.Price,
                    CurrentPrice = product?.Price ?? i.Price,
                    Quantity = i.Quantity,
                    Stock = product?.Stock ?? 0,
                    Subtotal = i.GetTotalPrice()
                };
            }).ToList()
        };
    }
}
