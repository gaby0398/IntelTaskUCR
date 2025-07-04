using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class AccionRepository : IAccionRepository
    {
        private readonly IntelTaskDbContext _context;

        public AccionRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EAccion>> GetAllAsync()
        {
            return await _context.T_Acciones.ToListAsync();
        }

        public async Task<EAccion?> GetByIdAsync(int id)
        {
            return await _context.T_Acciones.FindAsync(id);
        }

        public async Task<bool> AddAsync(EAccion accion)
        {
            try
            {
                await _context.T_Acciones.AddAsync(accion);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(EAccion accion)
        {
            try
            {
                _context.T_Acciones.Update(accion);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var accion = await _context.T_Acciones.FindAsync(id);
                if (accion == null) return false;

                _context.T_Acciones.Remove(accion);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        Task IAccionRepository.AddAsync(EAccion accion)
        {
            return AddAsync(accion);
        }
    }
}
