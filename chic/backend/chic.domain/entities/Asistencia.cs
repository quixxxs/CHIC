// chic.domain/entities/Asistencia.cs
namespace chic.domain;

public class Asistencia
{
    public int Id { get; set; }
    public TipoAsistencia Tipo { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.UtcNow;

    // Coordenadas que envió el usuario y distancia calculada a la sede
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public double DistanciaMetros { get; set; }

    public string? Observacion { get; set; }

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public int UbicacionId { get; set; }
    public Ubicacion? Ubicacion { get; set; }
}