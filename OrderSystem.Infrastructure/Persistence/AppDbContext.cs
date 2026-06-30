namespace OrderSystem.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using OrderSystem.Domain.Entities;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(p => p.Stock).IsRequired();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.UserId).IsRequired();
            entity.Property(o => o.CreatedAt).IsRequired();
            entity.HasMany(o => o.Items)
                  .WithOne()
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(oi => oi.Id);
            entity.Property(oi => oi.Quantity).IsRequired();
            entity.Property(oi => oi.Price).IsRequired().HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).IsRequired().HasDefaultValue("Client");
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            entity.OwnsOne(u => u.Address, a =>
            {
                a.Property(p => p.Street).HasColumnName("Address").HasMaxLength(250);
                a.Property(p => p.City).HasColumnName("City").HasMaxLength(100);
                a.Property(p => p.Department).HasColumnName("Department").HasMaxLength(100);
            });
            entity.Property(u => u.Phone).HasMaxLength(20);
        });
    }
}
