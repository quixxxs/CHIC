// chic.infrastructure/repositories/UbicacionRepository.cs
using Microsoft.EntityFrameworkCore;
using chic.application.interfaces;
using chic.domain;
using chic.infrastructure.data;

namespace chic.infrastructure.repositories;

public class UbicacionRepository : IUbicacionRepository
{
    private readonly ChicDbContext _context;
    public UbicacionRepository(ChicDbContext context) => _context = context;

    public async Task<IEnumerable<Ubicacion>> GetAllAsync() =>
        await _context.Ubicaciones.AsNoTracking().ToListAsync();

    public async Task<Ubicacion?> GetByIdAsync(int id) =>
        await _context.Ubicaciones.FindAsync(id);

    public async Task<Ubicacion> AddAsync(Ubicacion ubicacion)
    {
        await _context.Ubicaciones.AddAsync(ubicacion);
        await _context.SaveChangesAsync();
        return ubicacion;
    }

    public async Task UpdateAsync(Ubicacion ubicacion)
    {
        _context.Ubicaciones.Update(ubicacion);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Ubicacion ubicacion)
    {
        _context.Ubicaciones.Remove(ubicacion);
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<Ubicacion>> GetByEmpresaIdAsync(int empresaId) =>
        await _context.Ubicaciones.AsNoTracking().Where(u => u.EmpresaId == empresaId).ToListAsync();

    public async Task<bool> TieneAsistenciasAsync(int ubicacionId) =>
        await _context.Asistencias.AnyAsync(a => a.UbicacionId == ubicacionId);
}