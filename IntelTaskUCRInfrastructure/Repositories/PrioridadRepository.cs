using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class PrioridadRepository : IPrioridadRepository
    {
        private readonly IntelTaskDbContext _context;

        public PrioridadRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EPrioridad>> GetAllAsync()
        {
            return await _context.T_Prioridades.ToListAsync();
        }

        public async Task<EPrioridad?> GetByIdAsync(byte id)
        {
            return await _context.T_Prioridades.FindAsync(id);
        }

        public async Task AddAsync(EPrioridad prioridad)
        {
            await _context.T_Prioridades.AddAsync(prioridad);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EPrioridad prioridad)
        {
            _context.T_Prioridades.Update(prioridad);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(byte id)
        {
            var prioridad = await _context.T_Prioridades.FindAsync(id);
            if (prioridad != null)
            {
                _context.T_Prioridades.Remove(prioridad);
                await _context.SaveChangesAsync();
            }
        }
    }
}
