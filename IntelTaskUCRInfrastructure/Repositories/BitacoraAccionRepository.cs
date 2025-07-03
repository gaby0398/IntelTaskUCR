using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class BitacoraAccionRepository : IBitacoraAccionRepository
    {
        private readonly IntelTaskDbContext _context;

        public BitacoraAccionRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EBitacoraAccion>> GetAllAsync()
        {
            return await _context.T_Bitacora_Acciones.ToListAsync();
        }

        public async Task<EBitacoraAccion?> GetByIdAsync(int id)
        {
            return await _context.T_Bitacora_Acciones.FindAsync(id);
        }

        public async Task AddAsync(EBitacoraAccion bitacora)
        {
            await _context.T_Bitacora_Acciones.AddAsync(bitacora);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EBitacoraAccion bitacora)
        {
            _context.T_Bitacora_Acciones.Update(bitacora);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.T_Bitacora_Acciones.FindAsync(id);
            if (entity != null)
            {
                _context.T_Bitacora_Acciones.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
