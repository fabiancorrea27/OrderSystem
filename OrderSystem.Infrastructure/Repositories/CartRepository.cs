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
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task Save(Cart cart)
    {
        var existing = await _context.Carts
            .FirstOrDefaultAsync(c => c.UserId == cart.UserId);

        if (existing is null)
        {
            await _context.Carts.AddAsync(cart);
        }
        else
        {
            cart.SetId(existing.Id);

            foreach (var item in cart.Items)
                item.SetCartId(existing.Id);

            await using var tx = await _context.Database.BeginTransactionAsync();

            await _context.CartItems
                .Where(ci => ci.CartId == existing.Id)
                .ExecuteDeleteAsync();

            // Detach tracked items so EF doesn't try to update deleted rows
            foreach (var entry in _context.ChangeTracker.Entries<CartItem>().ToList())
                entry.State = EntityState.Detached;

            foreach (var item in cart.Items)
                _context.Entry(item).State = EntityState.Added;

            await _context.SaveChangesAsync();
            await tx.CommitAsync();
        }
    }
}
