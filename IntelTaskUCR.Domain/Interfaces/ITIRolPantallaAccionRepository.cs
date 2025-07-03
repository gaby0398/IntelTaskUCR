using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface ITIRolPantallaAccionRepository
    {
        Task<IEnumerable<TIRolPantallaAccion>> GetAllAsync();
        Task<TIRolPantallaAccion?> GetByIdsAsync(int idPantalla, int idAccion, int idRol);
        Task AddAsync(TIRolPantallaAccion item);
        Task DeleteAsync(int idPantalla, int idAccion, int idRol);
    }
}
