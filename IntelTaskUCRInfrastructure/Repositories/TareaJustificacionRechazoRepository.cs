using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class TareaJustificacionRechazoRepository : ITareaJustificacionRechazoRepository
    {
        private readonly IntelTaskDbContext _context;

        public TareaJustificacionRechazoRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ETareaJustificacionRechazo>> GetAllAsync()
        {
            return await _context.T_Tareas_Justificacion_Rechazo.ToListAsync();
        }

        public async Task<ETareaJustificacionRechazo?> GetByIdAsync(int id)
        {
            return await _context.T_Tareas_Justificacion_Rechazo
                .FirstOrDefaultAsync(j => j.CN_Id_tarea_rechazo == id);
        }

        public async Task AddAsync(ETareaJustificacionRechazo justificacion)
        {
            var ultimoId = await _context.T_Tareas_Justificacion_Rechazo
                .OrderByDescending(j => j.CN_Id_tarea_rechazo)
                .Select(j => j.CN_Id_tarea_rechazo)
                .FirstOrDefaultAsync();

            justificacion.CN_Id_tarea_rechazo = ultimoId + 1;

            await _context.T_Tareas_Justificacion_Rechazo.AddAsync(justificacion);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(ETareaJustificacionRechazo justificacion)
        {
            _context.T_Tareas_Justificacion_Rechazo.Update(justificacion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var justificacion = await _context.T_Tareas_Justificacion_Rechazo
                .FirstOrDefaultAsync(j => j.CN_Id_tarea_rechazo == id);

            if (justificacion != null)
            {
                _context.T_Tareas_Justificacion_Rechazo.Remove(justificacion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ETareaJustificacionRechazo>> GetByTareaIdAsync(int tareaId)
        {
            return await _context.T_Tareas_Justificacion_Rechazo
                                 .Where(j => j.CN_Id_tarea == tareaId)
                                 .ToListAsync();
        }








    }
}
