// chic.application/features/empresas/DTOs/EmpresaDTOs.cs 
namespace chic.application.features.empresas.DTOs;

public class CreateEmpresaRequestDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string? Telefono { get; set; }
}

public class UpdateEmpresaRequestDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public bool Activa { get; set; }
}

public class EmpresaResponseDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public bool Activa { get; set; }
    public DateTime FechaCreacion { get; set; }
}