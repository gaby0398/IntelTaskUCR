using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class TIUsuarioOficinaRepository : ITIUsuarioOficinaRepository
    {
        private readonly IntelTaskDbContext _context;

        public TIUsuarioOficinaRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TIUsuarioOficina>> GetAllAsync()
        {
            return await _context.TI_Usuario_X_Oficina.ToListAsync();
        }

        public async Task<TIUsuarioOficina?> GetByIdsAsync(int idUsuario, int idOficina)
        {
            return await _context.TI_Usuario_X_Oficina
                .FirstOrDefaultAsync(x => x.CN_Id_usuario == idUsuario && x.CN_Codigo_oficina == idOficina);
        }

        public async Task AddAsync(TIUsuarioOficina item)
        {
            await _context.TI_Usuario_X_Oficina.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int idUsuario, int idOficina)
        {
            var entity = await GetByIdsAsync(idUsuario, idOficina);
            if (entity != null)
            {
                _context.TI_Usuario_X_Oficina.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
