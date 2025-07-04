using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class AdjuntoXTareaRepository : IAdjuntoXTareaRepository
    {
        private readonly IntelTaskDbContext _context;

        public AdjuntoXTareaRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EAdjuntoXTarea>> GetAllAsync()
        {
            return await _context.T_Adjuntos_X_Tareas.ToListAsync();
        }

        public async Task<IEnumerable<EAdjuntoXTarea>> GetByTareaIdAsync(int idTarea)
        {
            return await _context.T_Adjuntos_X_Tareas
                .Where(x => x.CN_Id_tarea == idTarea)
                .ToListAsync();
        }

        public async Task AddAsync(EAdjuntoXTarea entity)
        {
            await _context.T_Adjuntos_X_Tareas.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int idTarea, int idAdjunto)
        {
            var item = await _context.T_Adjuntos_X_Tareas
                .FirstOrDefaultAsync(x => x.CN_Id_tarea == idTarea && x.CN_Id_adjuntos == idAdjunto);

            if (item != null)
            {
                _context.T_Adjuntos_X_Tareas.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
