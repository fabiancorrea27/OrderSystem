namespace OrderSystem.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Interfaces;
using OrderSystem.Infrastructure.Persistence;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByUserId(Guid userId)
    {
        return await _context.Carts
            .AsNoTracking()
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task Save(Cart cart)
    {
        var existing = await _context.Carts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == cart.UserId);

        if (existing is null)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return;
        }

        cart.SetId(existing.Id);

        await _context.CartItems
            .Where(ci => ci.CartId == existing.Id)
            .ExecuteDeleteAsync();

        foreach (var item in cart.Items)
        {
            var fresh = new CartItem(item.ProductId, item.Quantity, item.Price);
            fresh.SetCartId(existing.Id);
            _context.CartItems.Add(fresh);
        }

        await _context.SaveChangesAsync();
    }
}
