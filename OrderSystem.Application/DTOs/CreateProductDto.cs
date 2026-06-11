using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Application.DTOs;

public class CreateProductDto
{
    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(200, ErrorMessage = "El nombre del producto no puede tener más de 200 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
    public decimal Price { get; set; }
}
