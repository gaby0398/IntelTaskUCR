using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IDiaNoHabilRepository
    {
        Task<IEnumerable<EDiaNoHabil>> GetAllAsync();
        Task<EDiaNoHabil?> GetByIdAsync(int id);
        Task AddAsync(EDiaNoHabil dia);
        Task UpdateAsync(EDiaNoHabil dia);
        Task DeleteAsync(int id);
    }
}
