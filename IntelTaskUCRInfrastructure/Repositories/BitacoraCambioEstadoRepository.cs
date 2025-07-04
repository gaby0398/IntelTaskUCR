using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class BitacoraCambioEstadoRepository : IBitacoraCambioEstadoRepository
    {
        private readonly IntelTaskDbContext _context;

        public BitacoraCambioEstadoRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EBitacoraCambioEstado>> GetAllAsync()
        {
            return await _context.T_Bitacora_Cambios_Estados.ToListAsync();
        }

        public async Task<EBitacoraCambioEstado?> GetByIdAsync(int id)
        {
            return await _context.T_Bitacora_Cambios_Estados.FindAsync(id);
        }

        public async Task AddAsync(EBitacoraCambioEstado cambio)
        {
            await _context.T_Bitacora_Cambios_Estados.AddAsync(cambio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EBitacoraCambioEstado cambio)
        {
            _context.T_Bitacora_Cambios_Estados.Update(cambio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.T_Bitacora_Cambios_Estados.FindAsync(id);
            if (entity != null)
            {
                _context.T_Bitacora_Cambios_Estados.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<EBitacoraCambioEstado>> GetByTareaIdAsync(int tareaId)
        {
            return await _context.T_Bitacora_Cambios_Estados
                .Where(b => b.CN_Id_tarea_permiso == tareaId)
                .OrderByDescending(b => b.CF_Fecha_hora_cambio)
                .ToListAsync();
        }

    }
}
