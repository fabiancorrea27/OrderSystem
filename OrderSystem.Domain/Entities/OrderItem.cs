namespace OrderSystem.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    
    private OrderItem() { }

    public OrderItem(Guid productId, int quantity, decimal price)
    {
        Id = Guid.NewGuid();
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }

    public decimal GetTotalPrice() => Quantity * Price;
}