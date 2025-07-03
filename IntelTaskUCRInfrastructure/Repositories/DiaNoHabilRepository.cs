using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class DiaNoHabilRepository : IDiaNoHabilRepository
    {
        private readonly IntelTaskDbContext _context;

        public DiaNoHabilRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EDiaNoHabil>> GetAllAsync()
        {
            return await _context.T_Dias_No_Habiles.ToListAsync();
        }

        public async Task<EDiaNoHabil?> GetByIdAsync(int id)
        {
            return await _context.T_Dias_No_Habiles.FindAsync(id);
        }

        public async Task AddAsync(EDiaNoHabil dia)
        {
            await _context.T_Dias_No_Habiles.AddAsync(dia);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EDiaNoHabil dia)
        {
            _context.T_Dias_No_Habiles.Update(dia);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.T_Dias_No_Habiles.FindAsync(id);
            if (entity != null)
            {
                _context.T_Dias_No_Habiles.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
