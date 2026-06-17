namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Interfaces;

public class CreateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public CreateOrderUseCase(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<OrderResponseDto> Execute(CreateOrderDto dto, Guid userId)
    {
        var order = new Order(userId);

        foreach (var item in dto.Items)
        {
            var product = await _productRepository.GetById(item.ProductId);

            if (product is null)
                throw new KeyNotFoundException($"Product with id {item.ProductId} not found.");

            if (item.Quantity > product.Stock)
                throw new InvalidOperationException($"No hay suficiente stock de '{product.Name}'. Stock disponible: {product.Stock}.");

            product.DecreaseStock(item.Quantity);
            await _productRepository.Update(product);
            order.AddItem(product.Id, item.Quantity, product.Price);
        }

        await _orderRepository.Add(order);

        return new OrderResponseDto
        {
            Id = order.Id,
            UserId = order.UserId,
            CreatedAt = order.CreatedAt,
            Total = order.CalculateTotal(),
            Items = order.Items.Select(i => new OrderItemResponseDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price,
                Subtotal = i.GetTotalPrice()
            }).ToList()
        };
    }
}
