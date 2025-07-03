using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IComplejidadRepository
    {
        Task<IEnumerable<EComplejidad>> GetAllAsync();
        Task<EComplejidad?> GetByIdAsync(int id);
        Task AddAsync(EComplejidad complejidad);
        Task UpdateAsync(EComplejidad complejidad);
        Task DeleteAsync(int id);
    }
}
