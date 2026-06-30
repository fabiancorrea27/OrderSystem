using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Application.DTOs;

public class AddressDto
{
    [StringLength(250, ErrorMessage = "La dirección no puede tener más de 250 caracteres.")]
    public string? Street { get; set; }

    [StringLength(100, ErrorMessage = "La ciudad no puede tener más de 100 caracteres.")]
    public string? City { get; set; }

    [StringLength(100, ErrorMessage = "El departamento no puede tener más de 100 caracteres.")]
    public string? Department { get; set; }
}
