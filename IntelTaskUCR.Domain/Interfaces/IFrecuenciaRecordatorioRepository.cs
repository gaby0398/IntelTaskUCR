using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IFrecuenciaRecordatorioRepository
    {
        Task<IEnumerable<EFrecuenciaRecordatorio>> GetAllAsync();
        Task<EFrecuenciaRecordatorio?> GetByIdAsync(int id);
        Task AddAsync(EFrecuenciaRecordatorio recordatorio);
        Task UpdateAsync(EFrecuenciaRecordatorio recordatorio);
        Task DeleteAsync(int id);
    }
}
