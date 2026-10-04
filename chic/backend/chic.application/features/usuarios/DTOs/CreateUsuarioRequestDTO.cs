// chic.application/features/usuarios/DTOs/CreateUsuarioRequestDTO.cs
namespace chic.application.features.usuarios.DTOs;

public class CreateUsuarioRequestDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Cargo { get; set; }
    public int EmpresaId { get; set; }
}