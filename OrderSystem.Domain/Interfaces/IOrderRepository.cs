namespace OrderSystem.Domain.Interfaces;

using OrderSystem.Domain.Entities;

public interface IOrderRepository
{
    Task Add(Order order);
    Task<Order?> GetById(Guid id);
    Task<List<Order>> GetByUserId(Guid userId);
}
