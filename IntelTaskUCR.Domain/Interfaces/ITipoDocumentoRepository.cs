using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface ITipoDocumentoRepository
    {
        Task<IEnumerable<ETipoDocumento>> GetAllAsync();
        Task<ETipoDocumento?> GetByIdAsync(int id);
        Task AddAsync(ETipoDocumento tipo);
        Task UpdateAsync(ETipoDocumento tipo);
        Task DeleteAsync(int id);
    }
}
