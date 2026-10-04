// chic.application/Interfaces/IchicRepository.cs
using chic.domain;

namespace chic.application.interfaces;

public interface IChicRepository {
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<Usuario> AddAsync(Usuario usuario); // Método para POST agregar un nuevo usuario
    Task<Usuario?> GetByIdAsync(int id); // Método para GET por ID null por si no existe
    Task UpdateAsync(Usuario usuario); // Método para PUT actualizar un usuario existente
    Task DeleteAsync(Usuario usuario); // Método para DELETE eliminar un usuario
    Task<bool> ExisteCorreoAsync(string correo, int? excluirId = null);
    Task<bool> TieneAsistenciasAsync(int usuarioId);
}