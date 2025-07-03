using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class DemoRepository : IDemoRepository
    {
        private readonly IntelTaskDbContext _context;

        public DemoRepository(IntelTaskDbContext context) { _context = context;  }

        public async Task<IEnumerable<EDemo>> GetAllAsync()
        {
            return await _context.T_Demo.ToListAsync();
        }

        public async Task<EDemo?> GetByIdAsync(int id)
        {
            return await _context.T_Demo.FindAsync(id);
        }

        public async Task AddAsync(EDemo demo)
        {
            await _context.T_Demo.AddAsync(demo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EDemo demo)
        {
            _context.T_Demo.Update(demo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entidad = await _context.T_Demo.FindAsync(id);

                if (entidad!= null) {

                _context.T_Demo.Remove(entidad);
                await _context.SaveChangesAsync();
            }

            
        }
    }
}
