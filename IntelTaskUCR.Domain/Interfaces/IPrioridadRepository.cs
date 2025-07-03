using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IPrioridadRepository
    {
        Task<IEnumerable<EPrioridad>> GetAllAsync();
        Task<EPrioridad?> GetByIdAsync(byte id);
        Task AddAsync(EPrioridad prioridad);
        Task UpdateAsync(EPrioridad prioridad);
        Task DeleteAsync(byte id);
    }
}
