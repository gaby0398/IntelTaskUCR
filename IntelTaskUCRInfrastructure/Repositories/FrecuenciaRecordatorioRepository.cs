using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class FrecuenciaRecordatorioRepository : IFrecuenciaRecordatorioRepository
    {
        private readonly IntelTaskDbContext _context;

        public FrecuenciaRecordatorioRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EFrecuenciaRecordatorio>> GetAllAsync()
        {
            return await _context.T_Frecuencia_Recordatorio.ToListAsync();
        }

        public async Task<EFrecuenciaRecordatorio?> GetByIdAsync(int id)
        {
            return await _context.T_Frecuencia_Recordatorio.FindAsync(id);
        }

        public async Task AddAsync(EFrecuenciaRecordatorio recordatorio)
        {
            await _context.T_Frecuencia_Recordatorio.AddAsync(recordatorio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EFrecuenciaRecordatorio recordatorio)
        {
            _context.T_Frecuencia_Recordatorio.Update(recordatorio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.T_Frecuencia_Recordatorio.FindAsync(id);
            if (entity != null)
            {
                _context.T_Frecuencia_Recordatorio.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
