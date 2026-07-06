using OrderSystem.Domain.ValueObjects;

namespace OrderSystem.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<OrderItem> Items { get; private set; } = new();
    public Address? ShippingAddress { get; private set; }

    private Order() { }

    public Order(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(Guid productId, int quantity, decimal price)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        Items.Add(new OrderItem(productId, quantity, price));
    }

    public void SetShippingAddress(Address? shippingAddress)
    {
        ShippingAddress = shippingAddress;
    }

    public decimal CalculateTotal() => Items.Sum(i => i.GetTotalPrice());
}
