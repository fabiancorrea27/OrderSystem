using OrderSystem.Domain.ValueObjects;

namespace OrderSystem.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Client";
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<Address> SavedAddresses { get; set; } = new();
    public string? Phone { get; set; }
}
