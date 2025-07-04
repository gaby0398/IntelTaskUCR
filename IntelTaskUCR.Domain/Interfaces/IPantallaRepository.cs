using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IPantallaRepository
    {
        Task<IEnumerable<EPantalla>> GetAllAsync();
        Task<EPantalla?> GetByIdAsync(int id);
        Task AddAsync(EPantalla pantalla);
        Task UpdateAsync(EPantalla pantalla);
        Task DeleteAsync(int id);
    }
}
