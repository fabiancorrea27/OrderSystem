namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Interfaces;
using OrderSystem.Domain.ValueObjects;

public class UpdateProfileUseCase
{
    private readonly IUserRepository _repository;

    public UpdateProfileUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserProfileDto> Execute(Guid userId, UpdateProfileDto dto)
    {
        var user = await _repository.GetById(userId);
        if (user is null)
            throw new KeyNotFoundException("User not found.");

        if (!string.Equals(user.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _repository.GetByEmail(dto.Email);
            if (existing is not null)
                throw new InvalidOperationException("A user with this email already exists.");
        }

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.Phone = dto.Phone;
        user.Address = dto.Address is not null
            ? new Address(dto.Address.Street, dto.Address.City, dto.Address.Department)
            : null;

        await _repository.Update(user);

        return new UserProfileDto
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Address = user.Address is not null
                ? new AddressDto
                {
                    Street = user.Address.Street,
                    City = user.Address.City,
                    Department = user.Address.Department
                }
                : null,
            Phone = user.Phone
        };
    }
}
