namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Interfaces;

public class GetMyOrdersUseCase
{
    private readonly IOrderRepository _orderRepository;

    public GetMyOrdersUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderResponseDto>> Execute(Guid userId)
    {
        var orders = await _orderRepository.GetByUserId(userId);

        return orders.Select(order => new OrderResponseDto
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
        }).ToList();
    }
}
