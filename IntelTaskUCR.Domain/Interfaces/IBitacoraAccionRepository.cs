using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IBitacoraAccionRepository
    {
        Task<IEnumerable<EBitacoraAccion>> GetAllAsync();
        Task<EBitacoraAccion?> GetByIdAsync(int id);
        Task AddAsync(EBitacoraAccion bitacora);
        Task UpdateAsync(EBitacoraAccion bitacora);
        Task DeleteAsync(int id);
    }
}
