using Moq;
using OrderSystem.Application.DTOs;
using OrderSystem.Application.UseCases;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Interfaces;
using OrderSystem.Domain.ValueObjects;

namespace OrderSystem.Tests;

public class UnitTest
{
    [Fact]
    public void CalculateTotal_WithMultipleItems_ReturnsCorrectSum()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), quantity: 2, price: 10.50m);
        order.AddItem(Guid.NewGuid(), quantity: 1, price: 5.25m);
        order.AddItem(Guid.NewGuid(), quantity: 3, price: 2.00m);

        // Act
        var total = order.CalculateTotal();

        // Assert
        Assert.Equal(32.25m, total);
    }

    [Fact]
    public void AddItem_WithZeroOrNegativeQuantity_ThrowsArgumentException()
    {
        // Arrange
        var order = new Order(Guid.NewGuid());

        // Act & Assert
        Assert.Throws<ArgumentException>(() => order.AddItem(Guid.NewGuid(), quantity: 0, price: 10.0m));
        Assert.Throws<ArgumentException>(() => order.AddItem(Guid.NewGuid(), quantity: -1, price: 10.0m));
    }

    [Fact]
    public async Task CreateOrderUseCase_WithValidItems_AddsOrderAndReturnsOrderDto()
    {
        // Arrange
        var requestedProductId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var product = new Product("Test Product", 10.50m, 10);

        var productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock
            .Setup(x => x.GetById(requestedProductId))
            .ReturnsAsync(product);

        var orderRepositoryMock = new Mock<IOrderRepository>();
        orderRepositoryMock
            .Setup(x => x.Add(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        var useCase = new CreateOrderUseCase(orderRepositoryMock.Object, productRepositoryMock.Object);
        var dto = new CreateOrderDto
        {
            Items = new List<CreateOrderItemDto>
            {
                new() { ProductId = requestedProductId, Quantity = 2 }
            }
        };

        // Act
        var response = await useCase.Execute(dto, userId);

        // Assert
        Assert.Equal(userId, response.UserId);
        Assert.Equal(21.00m, response.Total);
        Assert.Single(response.Items);
        Assert.Equal(product.Id, response.Items[0].ProductId);
        Assert.Equal(2, response.Items[0].Quantity);
        Assert.Equal(10.50m, response.Items[0].Price);
        Assert.Equal(21.00m, response.Items[0].Subtotal);

        orderRepositoryMock.Verify(x => x.Add(It.IsAny<Order>()), Times.Once);
        productRepositoryMock.Verify(x => x.GetById(requestedProductId), Times.Once);
    }

    [Fact]
    public async Task GetMyOrdersUseCase_WhenOrderHasShippingAddress_ReturnsItInResponse()
    {
        // Arrange
        var orderRepositoryMock = new Mock<IOrderRepository>();
        var order = new Order(Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), quantity: 1, price: 10.00m);
        order.SetShippingAddress(new Address("Calle 123", "Bogotá", "Cundinamarca"));

        orderRepositoryMock
            .Setup(x => x.GetByUserId(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Order> { order });

        var useCase = new GetMyOrdersUseCase(orderRepositoryMock.Object);

        // Act
        var response = await useCase.Execute(Guid.NewGuid());

        // Assert
        Assert.Single(response);
        Assert.NotNull(response[0].ShippingAddress);
        Assert.Equal("Calle 123", response[0].ShippingAddress!.Street);
        Assert.Equal("Bogotá", response[0].ShippingAddress.City);
        Assert.Equal("Cundinamarca", response[0].ShippingAddress.Department);
    }

    [Fact]
    public async Task RegisterUserUseCase_WhenEmailAlreadyExists_ThrowsInvalidOperationException()
    {
        // Arrange
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "existing@example.com",
            PasswordHash = "hash",
            Role = "Client"
        };

        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock
            .Setup(x => x.GetByEmail(existingUser.Email))
            .ReturnsAsync(existingUser);

        var useCase = new RegisterUserUseCase(userRepositoryMock.Object);
        var dto = new RegisterUserDto
        {
            Email = existingUser.Email,
            Password = "Password123!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.Execute(dto));
        userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
    }
}
