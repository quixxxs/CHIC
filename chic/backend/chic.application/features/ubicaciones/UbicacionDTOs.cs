// chic.application/features/ubicaciones/DTOs/UbicacionDTOs.cs
namespace chic.application.features.ubicaciones.DTOs;

public class CreateUbicacionRequestDTO
{
    public string Nombre { get; set; } = string.Empty;
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public int RadioMetros { get; set; }
    public int EmpresaId { get; set; }
}

public class UpdateUbicacionRequestDto
{
    public string Nombre { get; set; } = string.Empty;
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public int RadioMetros { get; set; }
}

public class UbicacionResponseDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public int RadioMetros { get; set; }
    public int EmpresaId { get; set; }
}