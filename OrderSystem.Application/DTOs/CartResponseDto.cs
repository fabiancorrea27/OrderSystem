namespace OrderSystem.Application.DTOs;

public class CartResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public List<CartItemResponseDto> Items { get; set; } = new();
}

public class CartItemResponseDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal CurrentPrice { get; set; }
    public int Quantity { get; set; }
    public int Stock { get; set; }
    public decimal Subtotal { get; set; }
}
