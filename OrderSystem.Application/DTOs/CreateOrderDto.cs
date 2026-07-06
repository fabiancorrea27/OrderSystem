using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Application.DTOs;

public class CreateOrderDto
{
    [Required(ErrorMessage = "La orden debe contener al menos un artículo.")]
    [MinLength(1, ErrorMessage = "La orden debe contener al menos un artículo.")]
    public List<CreateOrderItemDto> Items { get; set; } = new();

    public AddressDto? ShippingAddress { get; set; }
}

public class CreateOrderItemDto
{
    [Required(ErrorMessage = "El id del producto es obligatorio.")]
    public Guid ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
    public int Quantity { get; set; }
}
