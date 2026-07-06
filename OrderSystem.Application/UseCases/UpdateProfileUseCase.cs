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

        user.SavedAddresses = (dto.Addresses ?? new List<AddressDto>())
            .Select(a => new Address(a.Street, a.City, a.Department))
            .Where(a => a.HasValue)
            .ToList();

        await _repository.Update(user);

        return new UserProfileDto
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Addresses = user.SavedAddresses
                .Where(a => a.HasValue)
                .Select(a => new AddressDto
                {
                    Street = a.Street,
                    City = a.City,
                    Department = a.Department
                })
                .ToList(),
            Phone = user.Phone
        };
    }
}
