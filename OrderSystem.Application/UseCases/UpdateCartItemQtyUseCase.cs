namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Interfaces;

public class UpdateCartItemQtyUseCase
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly GetCartUseCase _getCart;

    public UpdateCartItemQtyUseCase(ICartRepository cartRepository, IProductRepository productRepository, GetCartUseCase getCart)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _getCart = getCart;
    }

    public async Task<CartResponseDto?> Execute(Guid productId, int quantity, Guid userId)
    {
        var cart = await _cartRepository.GetByUserId(userId)
            ?? throw new KeyNotFoundException("Cart not found.");

        var product = await _productRepository.GetById(productId)
            ?? throw new KeyNotFoundException($"Product {productId} not found.");

        if (quantity > product.Stock)
            throw new InvalidOperationException($"Not enough stock. Available: {product.Stock}.");

        cart.UpdateItemQty(productId, quantity);
        await _cartRepository.Save(cart);

        return await _getCart.ToDto(cart);
    }
}
