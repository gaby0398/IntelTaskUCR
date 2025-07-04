using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface ITareaJustificacionRechazoRepository
    {
        Task<IEnumerable<ETareaJustificacionRechazo>> GetAllAsync();
        Task<ETareaJustificacionRechazo?> GetByIdAsync(int id);
        Task AddAsync(ETareaJustificacionRechazo justificacion);
        Task UpdateAsync(ETareaJustificacionRechazo justificacion);
        Task DeleteAsync(int id);

        Task<IEnumerable<ETareaJustificacionRechazo>> GetByTareaIdAsync(int tareaId);

    }
}
