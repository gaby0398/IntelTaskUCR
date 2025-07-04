using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class TipoDocumentoRepository : ITipoDocumentoRepository
    {
        private readonly IntelTaskDbContext _context;

        public TipoDocumentoRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ETipoDocumento>> GetAllAsync()
        {
            return await _context.T_Tipos_documentos.ToListAsync();
        }

        public async Task<ETipoDocumento?> GetByIdAsync(int id)
        {
            return await _context.T_Tipos_documentos.FindAsync(id);
        }

        public async Task AddAsync(ETipoDocumento tipo)
        {
            await _context.T_Tipos_documentos.AddAsync(tipo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ETipoDocumento tipo)
        {
            _context.T_Tipos_documentos.Update(tipo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tipo = await _context.T_Tipos_documentos.FindAsync(id);
            if (tipo != null)
            {
                _context.T_Tipos_documentos.Remove(tipo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
