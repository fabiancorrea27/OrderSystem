using Moq;
using OrderSystem.Application.DTOs;
using OrderSystem.Application.UseCases;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Interfaces;

namespace OrderSystem.Tests;

public class UpdateProfileUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldPersistMultipleSavedAddresses()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@example.com",
            PasswordHash = "hash",
            FirstName = "Ana",
            LastName = "Lopez"
        };

        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetById(user.Id)).ReturnsAsync(user);
        repository.Setup(r => r.GetByEmail("new@example.com")).ReturnsAsync((User?)null);
        repository.Setup(r => r.Update(It.IsAny<User>())).Returns(Task.CompletedTask);

        var useCase = new UpdateProfileUseCase(repository.Object);
        var dto = new UpdateProfileDto
        {
            FirstName = "Ana",
            LastName = "Lopez",
            Email = "new@example.com",
            Addresses = new List<AddressDto>
            {
                new() { Street = "Casa", City = "Bogotá", Department = "Cundinamarca" },
                new() { Street = "Trabajo", City = "Bogotá", Department = "Cundinamarca" }
            }
        };

        await useCase.Execute(user.Id, dto);

        Assert.Equal(2, user.SavedAddresses.Count);
        Assert.Equal("Casa", user.SavedAddresses[0].Street);
        Assert.Equal("Trabajo", user.SavedAddresses[1].Street);
    }
}
