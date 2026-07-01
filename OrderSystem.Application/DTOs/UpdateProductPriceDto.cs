using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Application.DTOs;

public class UpdateProductPriceDto
{
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
    public decimal Price { get; set; }
}
