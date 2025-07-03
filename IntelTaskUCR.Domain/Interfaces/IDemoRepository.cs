using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IDemoRepository
    {
        Task<IEnumerable<EDemo>> GetAllAsync();

        Task<EDemo?> GetByIdAsync(int id);

        Task AddAsync(EDemo demo);

        Task UpdateAsync(EDemo demo);

        Task DeleteAsync(int id);

    }
}
