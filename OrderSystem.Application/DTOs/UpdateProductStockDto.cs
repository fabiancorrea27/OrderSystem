using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Application.DTOs;

public class UpdateProductStockDto
{
    [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser 0 o mayor.")]
    public int Stock { get; set; }
}