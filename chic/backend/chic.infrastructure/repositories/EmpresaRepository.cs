// chic.infrastructure/repositories/EmpresaRepository.cs
using Microsoft.EntityFrameworkCore;
using chic.application.interfaces;
using chic.domain;
using chic.infrastructure.data;

namespace chic.infrastructure.repositories;

public class EmpresaRepository : IEmpresaRepository
{
    private readonly ChicDbContext _context;
    public EmpresaRepository(ChicDbContext context) => _context = context;

    public async Task<IEnumerable<Empresa>> GetAllAsync() =>
        await _context.Empresas.AsNoTracking().ToListAsync();

    public async Task<Empresa?> GetByIdAsync(int id) =>
        await _context.Empresas.FindAsync(id);

    public async Task<Empresa> AddAsync(Empresa empresa)
    {
        await _context.Empresas.AddAsync(empresa);
        await _context.SaveChangesAsync();
        return empresa;
    }

    public async Task UpdateAsync(Empresa empresa)
    {
        _context.Empresas.Update(empresa);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Empresa empresa)
    {
        _context.Empresas.Remove(empresa);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _context.Empresas.AnyAsync(e => e.Id == id);

    public async Task<bool> TieneDependenciasAsync(int id) =>
        await _context.Usuarios.AnyAsync(u => u.EmpresaId == id)
        || await _context.Ubicaciones.AnyAsync(u => u.EmpresaId == id);
}