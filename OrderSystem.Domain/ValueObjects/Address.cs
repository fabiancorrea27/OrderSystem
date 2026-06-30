namespace OrderSystem.Domain.ValueObjects;

public class Address
{
    public string? Street { get; }
    public string? City { get; }
    public string? Department { get; }

    private Address() { }

    public Address(string? street, string? city, string? department)
    {
        Street = street?.Trim();
        City = city?.Trim();
        Department = department?.Trim();
    }

    public bool HasValue => !string.IsNullOrWhiteSpace(Street)
                          || !string.IsNullOrWhiteSpace(City)
                          || !string.IsNullOrWhiteSpace(Department);

    public override bool Equals(object? obj)
    {
        if (obj is not Address other) return false;
        return Street == other.Street && City == other.City && Department == other.Department;
    }

    public override int GetHashCode() => HashCode.Combine(Street, City, Department);

    public override string ToString()
    {
        var parts = new[] { Street, City, Department };
        return string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
    }
}
