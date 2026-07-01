namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Interfaces;

public class RemoveCartItemUseCase
{
    private readonly ICartRepository _cartRepository;
    private readonly GetCartUseCase _getCart;

    public RemoveCartItemUseCase(ICartRepository cartRepository, GetCartUseCase getCart)
    {
        _cartRepository = cartRepository;
        _getCart = getCart;
    }

    public async Task<CartResponseDto?> Execute(Guid productId, Guid userId)
    {
        var cart = await _cartRepository.GetByUserId(userId)
            ?? throw new KeyNotFoundException("Cart not found.");

        cart.RemoveItem(productId);
        await _cartRepository.Save(cart);

        return await _getCart.ToDto(cart);
    }
}
