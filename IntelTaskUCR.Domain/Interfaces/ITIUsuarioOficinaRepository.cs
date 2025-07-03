using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface ITIUsuarioOficinaRepository
    {
        Task<IEnumerable<TIUsuarioOficina>> GetAllAsync();
        Task<TIUsuarioOficina?> GetByIdsAsync(int idUsuario, int idOficina);
        Task AddAsync(TIUsuarioOficina item);
        Task DeleteAsync(int idUsuario, int idOficina);
    }
}
