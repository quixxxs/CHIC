// chic.infrastructure/repositories/chicRepository.cs
using Microsoft.EntityFrameworkCore;
using chic.domain;
using chic.application.interfaces;
using chic.infrastructure.data;

namespace chic.infrastructure.repositories;

public class chicRepository : IChicRepository
{
    private readonly ChicDbContext _context;

     // El constructor recibe el contexto de EF Core
    public chicRepository(ChicDbContext context){
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios.ToListAsync(); // Consulta real a BD (Magia SQL)
    }

    public async Task<Usuario> AddAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario); // Agrega el usuario al contexto
        await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
        return usuario; // Devuelve el usuario agregado
    }
    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id); // Busca el usuario por ID
    }
    public async Task UpdateAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario); // Actualiza el usuario en el contexto
        await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
    }
    public async Task DeleteAsync(Usuario usuario)
    {
        _context.Usuarios.Remove(usuario); // Elimina el usuario del contexto
        await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
    }
    public async Task<bool> ExisteCorreoAsync(string correo, int? excluirId = null) =>
        await _context.Usuarios.AnyAsync(u => u.Correo == correo && (excluirId == null || u.Id != excluirId));

    public async Task<bool> TieneAsistenciasAsync(int usuarioId) =>
        await _context.Asistencias.AnyAsync(a => a.UsuarioId == usuarioId);
}