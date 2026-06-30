namespace OrderSystem.Domain.Interfaces;

using OrderSystem.Domain.Entities;

public interface IUserRepository
{
    Task<User?> GetByEmail(string email);
    Task<User?> GetById(Guid id);
    Task Add(User user);
}
