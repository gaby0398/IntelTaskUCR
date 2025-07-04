using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class TIRolPantallaAccionRepository : ITIRolPantallaAccionRepository
    {
        private readonly IntelTaskDbContext _context;

        public TIRolPantallaAccionRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TIRolPantallaAccion>> GetAllAsync()
        {
            return await _context.TI_Rol_X_Pantalla_X_Accion.ToListAsync();
        }

        public async Task<TIRolPantallaAccion?> GetByIdsAsync(int idPantalla, int idAccion, int idRol)
        {
            return await _context.TI_Rol_X_Pantalla_X_Accion
                .FirstOrDefaultAsync(x => x.CN_Id_pantalla == idPantalla &&
                                          x.CN_Id_accion == idAccion &&
                                          x.CN_Id_rol == idRol);
        }

        public async Task AddAsync(TIRolPantallaAccion item)
        {
            await _context.TI_Rol_X_Pantalla_X_Accion.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int idPantalla, int idAccion, int idRol)
        {
            var entity = await GetByIdsAsync(idPantalla, idAccion, idRol);
            if (entity != null)
            {
                _context.TI_Rol_X_Pantalla_X_Accion.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
