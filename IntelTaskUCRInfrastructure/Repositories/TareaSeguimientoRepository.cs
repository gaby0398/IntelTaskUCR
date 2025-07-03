using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class TareaSeguimientoRepository : ITareaSeguimientoRepository
    {
        private readonly IntelTaskDbContext _context;

        public TareaSeguimientoRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ETareaSeguimiento>> GetAllAsync()
        {
            return await _context.T_Tareas_Seguimiento.ToListAsync();
        }

        public async Task<ETareaSeguimiento?> GetByIdAsync(int id)
        {
            return await _context.T_Tareas_Seguimiento.FindAsync(id);
        }

        public async Task AddAsync(ETareaSeguimiento seguimiento)
        {
            await _context.T_Tareas_Seguimiento.AddAsync(seguimiento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ETareaSeguimiento seguimiento)
        {
            _context.T_Tareas_Seguimiento.Update(seguimiento);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.T_Tareas_Seguimiento.FindAsync(id);
            if (entity != null)
            {
                _context.T_Tareas_Seguimiento.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ETareaSeguimiento>> GetPorTareaAsync(int idTarea)
        {
            return await _context.T_Tareas_Seguimiento
                .Where(s => s.CN_Id_tarea == idTarea)
                .ToListAsync();
        }

    }
}
