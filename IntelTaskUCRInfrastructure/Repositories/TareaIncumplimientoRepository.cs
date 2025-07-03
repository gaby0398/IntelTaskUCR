using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class TareaIncumplimientoRepository : ITareaIncumplimientoRepository
    {
        private readonly IntelTaskDbContext _context;

        public TareaIncumplimientoRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ETareaIncumplimiento>> GetAllAsync()
        {
            return await _context.T_Tareas_Incumplimientos.ToListAsync();
        }

        public async Task<ETareaIncumplimiento?> GetByIdAsync(int id)
        {
            return await _context.T_Tareas_Incumplimientos.FindAsync(id);
        }

        public async Task AddAsync(ETareaIncumplimiento entidad)
        {
            // Obtener el último ID existente
            var ultimoId = await _context.T_Tareas_Incumplimientos
                .OrderByDescending(t => t.CN_Id_tarea_incumplimiento)
                .Select(t => t.CN_Id_tarea_incumplimiento)
                .FirstOrDefaultAsync();

            entidad.CN_Id_tarea_incumplimiento = ultimoId + 1;

            await _context.T_Tareas_Incumplimientos.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(ETareaIncumplimiento tareaIncumplimiento)
        {
            _context.T_Tareas_Incumplimientos.Update(tareaIncumplimiento);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.T_Tareas_Incumplimientos.FindAsync(id);
            if (entity != null)
            {
                _context.T_Tareas_Incumplimientos.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ETareaIncumplimiento>> ObtenerPorTarea(int idTarea)
        {
            return await _context.T_Tareas_Incumplimientos
                .Where(i => i.CN_Id_tarea == idTarea)
                .ToListAsync();
        }

    }
}
