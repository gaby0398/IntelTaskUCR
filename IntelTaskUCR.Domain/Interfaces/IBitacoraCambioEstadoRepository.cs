using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IBitacoraCambioEstadoRepository
    {
        Task<IEnumerable<EBitacoraCambioEstado>> GetAllAsync();
        Task<EBitacoraCambioEstado?> GetByIdAsync(int id);
        Task AddAsync(EBitacoraCambioEstado cambio);
        Task UpdateAsync(EBitacoraCambioEstado cambio);
        Task DeleteAsync(int id);

        Task<IEnumerable<EBitacoraCambioEstado>> GetByTareaIdAsync(int tareaId);

    }
}
