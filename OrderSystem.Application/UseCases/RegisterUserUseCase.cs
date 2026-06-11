namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Interfaces;

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

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = hash,
            Role = "Client"
        };

        await _repository.Add(user);
    }
}
