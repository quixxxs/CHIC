// chic.domain/entities/Empresa.cs
namespace chic.domain;

public class Empresa
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public bool Activa { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public ICollection<Ubicacion> Ubicaciones { get; set; } = new List<Ubicacion>();
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}