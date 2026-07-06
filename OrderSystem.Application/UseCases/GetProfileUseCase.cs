namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Interfaces;

public class GetProfileUseCase
{
    private readonly IUserRepository _repository;

    public GetProfileUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserProfileDto?> Execute(Guid userId)
    {
        var user = await _repository.GetById(userId);
        if (user is null) return null;

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
