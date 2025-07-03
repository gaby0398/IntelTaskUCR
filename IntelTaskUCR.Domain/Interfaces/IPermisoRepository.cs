using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IPermisoRepository
    {
        Task<IEnumerable<EPermiso>> GetAllAsync();
        Task<EPermiso?> GetByIdAsync(int id);
        Task AddAsync(EPermiso permiso);
        Task UpdateAsync(EPermiso permiso);
        Task DeleteAsync(int id);

        Task<IEnumerable<EPermiso>> GetPermisosPorEstadoRegistradoAsync();

        Task<int> GetCantidadPermisosPorEstadoRegistradoAsync();
    }
}
