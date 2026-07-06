namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Interfaces;
using OrderSystem.Domain.ValueObjects;

public class RegisterUserUseCase
{
    private readonly IUserRepository _repository;

    public RegisterUserUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task Execute(RegisterUserDto dto)
    {
        var existing = await _repository.GetByEmail(dto.Email);
        if (existing is not null)
            throw new InvalidOperationException("A user with this email already exists.");

        var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var savedAddresses = (dto.Addresses ?? new List<AddressDto>())
            .Select(a => new Address(a.Street, a.City, a.Department))
            .Where(a => a.HasValue)
            .ToList();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = hash,
            Role = "Client",
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            SavedAddresses = savedAddresses,
            Phone = dto.Phone
        };

        await _repository.Add(user);
    }
}
