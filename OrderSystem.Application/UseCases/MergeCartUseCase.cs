namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Interfaces;

public class MergeCartUseCase
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly GetCartUseCase _getCart;

    public MergeCartUseCase(ICartRepository cartRepository, IProductRepository productRepository, GetCartUseCase getCart)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _getCart = getCart;
    }

    public async Task<CartResponseDto?> Execute(List<CartItemRequestDto> items, Guid userId)
    {
        if (items.Count == 0)
        {
            var existing = await _cartRepository.GetByUserId(userId);
            return existing is null ? null : await _getCart.ToDto(existing);
        }

        var cart = await _cartRepository.GetByUserId(userId) ?? new Domain.Entities.Cart(userId);

        foreach (var item in items)
        {
            var product = await _productRepository.GetById(item.ProductId);
            if (product is null) continue;

            cart.AddItem(product.Id, item.Quantity, product.Price);
        }

        await _cartRepository.Save(cart);
        return await _getCart.ToDto(cart);
    }
}
