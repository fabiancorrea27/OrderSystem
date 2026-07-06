namespace OrderSystem.Application.DTOs;

public class OrderResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemResponseDto> Items { get; set; } = new();
    public decimal Total { get; set; }
    public AddressDto? ShippingAddress { get; set; }
}

public class OrderItemResponseDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Subtotal { get; set; }
}
