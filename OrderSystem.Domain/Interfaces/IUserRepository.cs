namespace OrderSystem.Domain.Interfaces;

using OrderSystem.Domain.Entities;

public interface IUserRepository
{
    Task<User?> GetByEmail(string email);
    Task Add(User user);
}
