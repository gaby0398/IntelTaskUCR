using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class NotificacionRepository : INotificacionRepository
    {
        private readonly IntelTaskDbContext _context;

        public NotificacionRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ENotificacion>> GetAllAsync()
        {
            return await _context.T_Notificaciones.ToListAsync();
        }

        public async Task<ENotificacion?> GetByIdAsync(int id)
        {
            return await _context.T_Notificaciones.FindAsync(id);
        }

        public async Task AddAsync(ENotificacion notificacion)
        {
            await _context.T_Notificaciones.AddAsync(notificacion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ENotificacion notificacion)
        {
            _context.T_Notificaciones.Update(notificacion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var notificacion = await _context.T_Notificaciones.FindAsync(id);
            if (notificacion != null)
            {
                _context.T_Notificaciones.Remove(notificacion);
                await _context.SaveChangesAsync();
            }
        }
    }
}
