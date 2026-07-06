using System.Linq;
using System.Net;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderSystem.Domain.Entities;
using OrderSystem.Infrastructure;
using OrderSystem.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa tu token JWT en el formato: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Infrastructure (DB, repos, use cases)
builder.Services.AddInfrastructure(builder.Configuration);

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();

    if (!dbContext.Users.Any(u => u.Email == "admin@example.com"))
    {
        dbContext.Users.Add(new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Role = "Admin"
        });
    }

    if (!dbContext.Products.Any())
    {
        dbContext.Products.AddRange(
            new Product("Laptop Gamer", 3499000m, 20),
            new Product("Auriculares Bluetooth", 289000m, 15),
            new Product("Teclado mecánico", 189000m, 8),
            new Product("Mouse inalámbrico", 89000m, 30),
            new Product("Monitor 27 pulgadas", 699000m, 12),
            new Product("Webcam HD", 149000m, 18),
            new Product("Silla ergonómica", 549000m, 10),
            new Product("Impresora láser", 429000m, 7),
            new Product("SSD 1TB", 249000m, 25),
            new Product("Memoria RAM 16GB", 189000m, 22),
            new Product("Router Wi-Fi 6", 329000m, 14),
            new Product("Cargador USB-C", 69000m, 40),
            new Product("Smartwatch", 399000m, 16),
            new Product("Tablet Android", 549000m, 11),
            new Product("Consola portátil", 899000m, 9),
            new Product("Altavoces Bluetooth", 199000m, 20),
            new Product("Cámara de seguridad", 279000m, 13),
            new Product("Módem 5G", 389000m, 8),
            new Product("Proyector LED", 729000m, 6),
            new Product("Micrófono USB", 139000m, 17)
        );
    }

    dbContext.SaveChanges();
}

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        var problemDetails = new
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            title = "An error occurred while processing your request.",
            status = (int)HttpStatusCode.InternalServerError,
            detail = exception?.Message,
            instance = context.Request.Path
        };

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
