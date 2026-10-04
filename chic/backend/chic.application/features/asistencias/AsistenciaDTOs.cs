// chic.application/features/asistencias/DTOs/AsistenciaDTOs.cs
using chic.domain;
namespace chic.application.features.asistencias.DTOs;

public class CreateAsistenciaRequestDTO
{
    public int UsuarioId { get; set; }
    public TipoAsistencia Tipo { get; set; }
    public double Latitud { get; set; }
    public double Longitud { get; set; }
}

public class UpdateAsistenciaRequestDto
{
    public TipoAsistencia Tipo { get; set; }
    public string? Observacion { get; set; }
}

public class AsistenciaResponseDTO
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int UbicacionId { get; set; }
    public TipoAsistencia Tipo { get; set; }
    public DateTime FechaHora { get; set; }
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public double DistanciaMetros { get; set; }
    public string? Observacion { get; set; }
}