using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class OficinaRepository : IOficinaRepository
    {
        private readonly IntelTaskDbContext _context;

        public OficinaRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EOficina>> GetAllAsync()
        {
            return await _context.T_Oficinas.ToListAsync();
        }

        public async Task<EOficina?> GetByIdAsync(int id)
        {
            return await _context.T_Oficinas.FindAsync(id);
        }

        public async Task AddAsync(EOficina oficina)
        {
            await _context.T_Oficinas.AddAsync(oficina);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EOficina oficina)
        {
            _context.T_Oficinas.Update(oficina);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var oficina = await _context.T_Oficinas.FindAsync(id);
            if (oficina != null)
            {
                _context.T_Oficinas.Remove(oficina);
                await _context.SaveChangesAsync();
            }
        }
    }
}
