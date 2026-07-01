namespace OrderSystem.Domain.Interfaces;

using OrderSystem.Domain.Entities;

public interface ICartRepository
{
    Task<Cart?> GetByUserId(Guid userId);
    Task Save(Cart cart);
}
