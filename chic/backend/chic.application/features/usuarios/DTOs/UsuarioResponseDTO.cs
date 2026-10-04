// chic.application/features/usuarios/DTOs/UsuarioResponseDTO.cs
namespace chic.application.features.usuarios.DTOs;

public class UsuarioResponseDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Cargo { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int EmpresaId { get; set; }
}