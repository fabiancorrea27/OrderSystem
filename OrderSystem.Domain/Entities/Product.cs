namespace OrderSystem.Domain.Entities;

public class Product
{
    public Guid Id { get; private set;}
    public string Name { get; private set;}
    public decimal Price { get; private set;}
    public int Stock { get; private set;}

    private Product()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
        Price = 0m;
        Stock = 0;
    }

    public Product(string name, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));

        if (price <= 0)
            throw new ArgumentException("Product price must be greater than zero.", nameof(price));

        if (stock < 0)
            throw new ArgumentException("Product stock cannot be negative.", nameof(stock));

        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Stock = stock;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Product price must be greater than zero.", nameof(newPrice));

        Price = newPrice;
    }

    public void SetStock(int stock)
    {
        if (stock < 0)
            throw new ArgumentException("Product stock cannot be negative.", nameof(stock));

        Stock = stock;
    }

    public void IncreaseStock(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Stock increase amount must be greater than zero.", nameof(amount));

        Stock += amount;
    }

    public void DecreaseStock(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Stock reduction amount must be greater than zero.", nameof(amount));

        if (Stock < amount)
            throw new InvalidOperationException("No hay suficiente stock del producto.");

        Stock -= amount;
    }
}