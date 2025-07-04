using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface ITareaIncumplimientoRepository
    {
        Task<IEnumerable<ETareaIncumplimiento>> GetAllAsync();
        Task<ETareaIncumplimiento?> GetByIdAsync(int id);
        Task AddAsync(ETareaIncumplimiento tareaIncumplimiento);
        Task UpdateAsync(ETareaIncumplimiento tareaIncumplimiento);
        Task DeleteAsync(int id);
        Task<IEnumerable<ETareaIncumplimiento>> ObtenerPorTarea(int idTarea);

    }
}
