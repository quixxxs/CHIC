// chic.application/interfaces/IUbicacionRepository.cs
using chic.domain;
namespace chic.application.interfaces;

public interface IUbicacionRepository
{
    Task<IEnumerable<Ubicacion>> GetAllAsync();
    Task<Ubicacion?> GetByIdAsync(int id);
    Task<Ubicacion> AddAsync(Ubicacion ubicacion);
    Task UpdateAsync(Ubicacion ubicacion);
    Task DeleteAsync(Ubicacion ubicacion);
    Task<IEnumerable<Ubicacion>> GetByEmpresaIdAsync(int empresaId);
    Task<bool> TieneAsistenciasAsync(int ubicacionId);
}