using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface ITINotificacionUsuarioRepository
    {
        Task<IEnumerable<TINotificacionUsuario>> GetAllAsync();
        Task<TINotificacionUsuario?> GetByIdsAsync(int idNotificacion, int idUsuario);
        Task AddAsync(TINotificacionUsuario item);
        Task UpdateAsync(TINotificacionUsuario item);
        Task DeleteAsync(int idNotificacion, int idUsuario);
    }
}
