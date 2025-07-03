using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface ITareaSeguimientoRepository
    {
        Task<IEnumerable<ETareaSeguimiento>> GetAllAsync();
        Task<ETareaSeguimiento?> GetByIdAsync(int id);
        Task AddAsync(ETareaSeguimiento seguimiento);
        Task UpdateAsync(ETareaSeguimiento seguimiento);
        Task DeleteAsync(int id);
        Task<IEnumerable<ETareaSeguimiento>> GetPorTareaAsync(int idTarea);

    }
}
