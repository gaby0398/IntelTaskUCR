using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IAdjuntoXTareaRepository
    {
        Task<IEnumerable<EAdjuntoXTarea>> GetAllAsync();
        Task<IEnumerable<EAdjuntoXTarea>> GetByTareaIdAsync(int idTarea);
        Task AddAsync(EAdjuntoXTarea entity);
        //Task UpdateAsync(EAdjuntoXTarea entity);
        Task DeleteAsync(int idTarea, int idAdjunto);
    }
}
