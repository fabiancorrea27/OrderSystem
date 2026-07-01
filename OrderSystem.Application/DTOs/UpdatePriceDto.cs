using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Application.DTOs;

public class UpdatePriceDto
{
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }
}
