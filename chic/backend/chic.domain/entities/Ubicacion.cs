// chic.domain/entities/Ubicacion.cs
namespace chic.domain;

public class Ubicacion
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public int RadioMetros { get; set; }                  // distancia permitida para marcar

    public int EmpresaId { get; set; }
    public Empresa? Empresa { get; set; }
}