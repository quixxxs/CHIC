// chic.application/interfaces/IEmpresaRepository.cs
using chic.domain;
namespace chic.application.interfaces;

public interface IEmpresaRepository
{
    Task<IEnumerable<Empresa>> GetAllAsync();
    Task<Empresa?> GetByIdAsync(int id);
    Task<Empresa> AddAsync(Empresa empresa);
    Task UpdateAsync(Empresa empresa);
    Task DeleteAsync(Empresa empresa);
    Task<bool> ExistsAsync(int id);
    Task<bool> TieneDependenciasAsync(int id);   // ¿tiene usuarios o ubicaciones?
}