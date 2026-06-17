namespace OrderSystem.Domain.Interfaces;

using OrderSystem.Domain.Entities;

public interface IProductRepository
{
    Task Add(Product product);
    Task<List<Product>> GetAll();
    Task<Product?> GetById(Guid id);
    Task Update(Product product);
}
