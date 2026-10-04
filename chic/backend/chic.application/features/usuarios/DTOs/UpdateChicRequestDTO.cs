// chic.application/features/usuarios/DTOs/UpdateChicRequestDto.cs
namespace chic.application.features.usuarios.DTOs;

// Solo expone Nombre: el Id viene de la ruta, 
// así que el cliente no puede cambiarlo desde el cuerpo.
public class UpdateChicRequestDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Cargo { get; set; }
    public bool Activo { get; set; }
    public int EmpresaId { get; set; }
}