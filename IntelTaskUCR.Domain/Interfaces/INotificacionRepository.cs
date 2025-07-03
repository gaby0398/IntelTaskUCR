using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface INotificacionRepository
    {
        Task<IEnumerable<ENotificacion>> GetAllAsync();
        Task<ENotificacion?> GetByIdAsync(int id);
        Task AddAsync(ENotificacion notificacion);
        Task UpdateAsync(ENotificacion notificacion);
        Task DeleteAsync(int id);
    }
}
