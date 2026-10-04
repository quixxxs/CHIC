// chic.application/interfaces/IAsistenciaRepository.cs
using chic.domain;
namespace chic.application.interfaces;

public interface IAsistenciaRepository
{
    Task<IEnumerable<Asistencia>> GetAllAsync(int? usuarioId = null);
    Task<Asistencia?> GetByIdAsync(int id);
    Task<Asistencia?> GetUltimaPorUsuarioAsync(int usuarioId);
    Task<Asistencia> AddAsync(Asistencia asistencia);
    Task UpdateAsync(Asistencia asistencia);
    Task DeleteAsync(Asistencia asistencia);
}