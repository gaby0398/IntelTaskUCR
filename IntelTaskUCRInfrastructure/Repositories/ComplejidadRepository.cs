using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class ComplejidadRepository : IComplejidadRepository
    {
        private readonly IntelTaskDbContext _context;

        public ComplejidadRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EComplejidad>> GetAllAsync()
        {
            return await _context.T_Complejidades.ToListAsync();
        }

        public async Task<EComplejidad?> GetByIdAsync(int id)
        {
            return await _context.T_Complejidades.FindAsync(id);
        }

        public async Task AddAsync(EComplejidad complejidad)
        {
            await _context.T_Complejidades.AddAsync(complejidad);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EComplejidad complejidad)
        {
            _context.T_Complejidades.Update(complejidad);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.T_Complejidades.FindAsync(id);
            if (entity != null)
            {
                _context.T_Complejidades.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
