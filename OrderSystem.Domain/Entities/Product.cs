namespace OrderSystem.Domain.Entities;

public class Product
{
    public Guid Id { get; private set;}
    public string Name { get; private set;}
    public decimal Price { get; private set;}

    private Product()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
        Price = 0m;
    }

    public Product(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));

        if (price <= 0)
            throw new ArgumentException("Product price must be greater than zero.", nameof(price));

        Id = Guid.NewGuid();
        Name = name;
        Price = price;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Product price must be greater than zero.", nameof(newPrice));

        Price = newPrice;
    }
}