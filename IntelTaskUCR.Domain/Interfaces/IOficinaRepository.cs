using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IOficinaRepository
    {
        Task<IEnumerable<EOficina>> GetAllAsync();
        Task<EOficina?> GetByIdAsync(int id);
        Task AddAsync(EOficina oficina);
        Task UpdateAsync(EOficina oficina);
        Task DeleteAsync(int id);
    }
}
