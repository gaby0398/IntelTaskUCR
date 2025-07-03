using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IAdjuntoRepository
    {
        Task<IEnumerable<EAdjunto>> GetAllAsync();
        Task<EAdjunto?> GetByIdAsync(int id);
        Task AddAsync(EAdjunto adjunto);
        Task UpdateAsync(EAdjunto adjunto);
        Task DeleteAsync(int id);

        //Task<IEnumerable<EAdjunto>> ObtenerPorTareaAsync(int idTarea);
    }
}
