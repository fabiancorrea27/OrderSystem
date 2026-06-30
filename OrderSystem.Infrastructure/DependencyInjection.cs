namespace OrderSystem.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderSystem.Application.UseCases;
using OrderSystem.Domain.Interfaces;
using OrderSystem.Infrastructure.Persistence;
using OrderSystem.Infrastructure.Repositories;
using OrderSystem.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Repositories (Adapters)
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Services (Adapters)
        services.AddScoped<IJwtService, JwtService>();

        // Use Cases
        services.AddScoped<RegisterUserUseCase>();
        services.AddScoped<LoginUseCase>();
        services.AddScoped<CreateProductUseCase>();
        services.AddScoped<GetProductsUseCase>();
        services.AddScoped<UpdateProductStockUseCase>();
        services.AddScoped<CreateOrderUseCase>();
        services.AddScoped<GetMyOrdersUseCase>();
        services.AddScoped<GetProfileUseCase>();

        return services;
    }
}
