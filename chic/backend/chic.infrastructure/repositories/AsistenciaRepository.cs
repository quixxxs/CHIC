// chic.infrastructure/repositories/AsistenciaRepository.cs
using Microsoft.EntityFrameworkCore;
using chic.application.interfaces;
using chic.domain;
using chic.infrastructure.data;

namespace chic.infrastructure.repositories;

public class AsistenciaRepository : IAsistenciaRepository
{
    private readonly ChicDbContext _context;
    public AsistenciaRepository(ChicDbContext context) => _context = context;

    public async Task<IEnumerable<Asistencia>> GetAllAsync(int? usuarioId = null)
    {
        var query = _context.Asistencias.AsNoTracking();
        if (usuarioId.HasValue)
            query = query.Where(a => a.UsuarioId == usuarioId.Value);

        return await query.OrderByDescending(a => a.FechaHora).ToListAsync();
    }

    public async Task<Asistencia?> GetByIdAsync(int id) =>
        await _context.Asistencias.FindAsync(id);

    public async Task<Asistencia?> GetUltimaPorUsuarioAsync(int usuarioId) =>
        await _context.Asistencias.AsNoTracking()
            .Where(a => a.UsuarioId == usuarioId)
            .OrderByDescending(a => a.FechaHora)
            .FirstOrDefaultAsync();

    public async Task<Asistencia> AddAsync(Asistencia asistencia)
    {
        await _context.Asistencias.AddAsync(asistencia);
        await _context.SaveChangesAsync();
        return asistencia;
    }

    public async Task UpdateAsync(Asistencia asistencia)
    {
        _context.Asistencias.Update(asistencia);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Asistencia asistencia)
    {
        _context.Asistencias.Remove(asistencia);
        await _context.SaveChangesAsync();
    }
}