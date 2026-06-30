using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Application.DTOs;

public class RegisterUserDto
{
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(100, ErrorMessage = "El apellido no puede tener más de 100 caracteres.")]
    public string LastName { get; set; } = string.Empty;

    public AddressDto? Address { get; set; }

    [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
    [StringLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres.")]
    public string? Phone { get; set; }
}
