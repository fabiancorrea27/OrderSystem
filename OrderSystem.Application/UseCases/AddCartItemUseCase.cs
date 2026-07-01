namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Interfaces;

public class AddCartItemUseCase
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly GetCartUseCase _getCart;

    public AddCartItemUseCase(ICartRepository cartRepository, IProductRepository productRepository, GetCartUseCase getCart)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _getCart = getCart;
    }

    public async Task<CartResponseDto> Execute(CartItemRequestDto dto, Guid userId)
    {
        var product = await _productRepository.GetById(dto.ProductId)
            ?? throw new KeyNotFoundException($"Product {dto.ProductId} not found.");

        if (dto.Quantity > product.Stock)
            throw new InvalidOperationException($"Not enough stock. Available: {product.Stock}.");

        var cart = await _cartRepository.GetByUserId(userId);
        if (cart is null)
            cart = new Domain.Entities.Cart(userId);

        cart.AddItem(product.Id, dto.Quantity, product.Price);
        await _cartRepository.Save(cart);

        return await _getCart.ToDto(cart);
    }
}
