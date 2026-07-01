namespace OrderSystem.Domain.Entities;

public class Cart
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public List<CartItem> Items { get; private set; } = new();

    private Cart() { }

    public Cart(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        Id = Guid.NewGuid();
        UserId = userId;
    }

    public void AddItem(Guid productId, int quantity, decimal price)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        var existing = Items.FirstOrDefault(i => i.ProductId == productId);
        if (existing is not null)
        {
            existing.AddQuantity(quantity);
        }
        else
        {
            Items.Add(new CartItem(productId, quantity, price));
        }
    }

    public void UpdateItemQty(Guid productId, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        var item = Items.FirstOrDefault(i => i.ProductId == productId)
            ?? throw new KeyNotFoundException($"Product {productId} not in cart.");

        item.SetQuantity(quantity);
    }

    public void UpdateItemPrice(Guid productId, decimal price)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId)
            ?? throw new KeyNotFoundException($"Product {productId} not in cart.");

        item.SetPrice(price);
    }

    public void RemoveItem(Guid productId)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId)
            ?? throw new KeyNotFoundException($"Product {productId} not in cart.");

        Items.Remove(item);
    }

    public void Clear() => Items.Clear();

    public void SetId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        Id = id;
    }
}
