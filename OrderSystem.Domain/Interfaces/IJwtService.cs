namespace OrderSystem.Domain.Interfaces;

using OrderSystem.Domain.Entities;

public interface IJwtService
{
    string GenerateToken(User user);
}
