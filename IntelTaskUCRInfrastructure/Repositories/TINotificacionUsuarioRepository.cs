using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class TINotificacionUsuarioRepository : ITINotificacionUsuarioRepository
    {
        private readonly IntelTaskDbContext _context;

        public TINotificacionUsuarioRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TINotificacionUsuario>> GetAllAsync()
        {
            return await _context.TI_Notificaciones_X_Usuarios.ToListAsync();
        }

        public async Task<TINotificacionUsuario?> GetByIdsAsync(int idNotificacion, int idUsuario)
        {
            return await _context.TI_Notificaciones_X_Usuarios
                .FirstOrDefaultAsync(n => n.CN_Id_notificacion == idNotificacion && n.CN_Id_usuario == idUsuario);
        }

        public async Task AddAsync(TINotificacionUsuario item)
        {
            await _context.TI_Notificaciones_X_Usuarios.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TINotificacionUsuario item)
        {
            _context.TI_Notificaciones_X_Usuarios.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int idNotificacion, int idUsuario)
        {
            var entity = await GetByIdsAsync(idNotificacion, idUsuario);
            if (entity != null)
            {
                _context.TI_Notificaciones_X_Usuarios.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
