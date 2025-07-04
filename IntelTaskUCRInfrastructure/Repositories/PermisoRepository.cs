using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class PermisoRepository : IPermisoRepository
    {
        private readonly IntelTaskDbContext _context;

        public PermisoRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EPermiso>> GetAllAsync()
        {
            return await _context.T_Permisos.ToListAsync();
        }

        public async Task<EPermiso?> GetByIdAsync(int id)
        {
            return await _context.T_Permisos.FindAsync(id);
        }

        public async Task AddAsync(EPermiso permiso)
        {
            // Marcar las entidades relacionadas como existentes para evitar duplicados
            if (permiso.UsuarioCreador != null)
                _context.Entry(permiso.UsuarioCreador).State = EntityState.Unchanged;

            if (permiso.Estado != null)
                _context.Entry(permiso.Estado).State = EntityState.Unchanged;

            await _context.T_Permisos.AddAsync(permiso);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(EPermiso permiso)
        {
            _context.T_Permisos.Update(permiso);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var permiso = await _context.T_Permisos.FindAsync(id);
            if (permiso != null)
            {
                _context.T_Permisos.Remove(permiso);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<EPermiso>> GetPermisosPorEstadoRegistradoAsync()
        {
            return await _context.T_Permisos
                                 .Where(p => p.CN_Id_estado != null && p.CN_Id_estado == 1)  // Verifica que no sea nulo
                                 .ToListAsync();
        }

        public async Task<int> GetCantidadPermisosPorEstadoRegistradoAsync()
        {
            return await _context.T_Permisos
                                 .Where(p => p.CN_Id_estado == 1)  // Estado "Registrado"
                                 .CountAsync();  // Devuelve solo la cantidad
        }



    }
}
