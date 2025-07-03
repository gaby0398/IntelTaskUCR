using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class PantallaRepository : IPantallaRepository
    {
        private readonly IntelTaskDbContext _context;

        public PantallaRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EPantalla>> GetAllAsync()
        {
            return await _context.T_Pantallas.ToListAsync();
        }

        public async Task<EPantalla?> GetByIdAsync(int id)
        {
            return await _context.T_Pantallas.FindAsync(id);
        }

        public async Task AddAsync(EPantalla pantalla)
        {
            await _context.T_Pantallas.AddAsync(pantalla);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EPantalla pantalla)
        {
            _context.T_Pantallas.Update(pantalla);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pantalla = await _context.T_Pantallas.FindAsync(id);
            if (pantalla != null)
            {
                _context.T_Pantallas.Remove(pantalla);
                await _context.SaveChangesAsync();
            }
        }
    }
}
