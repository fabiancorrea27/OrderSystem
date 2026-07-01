namespace OrderSystem.Application.UseCases;

using OrderSystem.Domain.Interfaces;

public class ClearCartUseCase
{
    private readonly ICartRepository _cartRepository;

    public ClearCartUseCase(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task Execute(Guid userId)
    {
        var cart = await _cartRepository.GetByUserId(userId);
        if (cart is null) return;

        cart.Clear();
        await _cartRepository.Save(cart);
    }
}
