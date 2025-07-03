using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class AdjuntoRepository : IAdjuntoRepository
    {
        private readonly IntelTaskDbContext _context;

        public AdjuntoRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EAdjunto>> GetAllAsync()
        {
            return await _context.T_Adjuntos.ToListAsync();
        }

        public async Task<EAdjunto?> GetByIdAsync(int id)
        {
            return await _context.T_Adjuntos.FindAsync(id);
        }

        public async Task AddAsync(EAdjunto adjunto)
        {
            await _context.T_Adjuntos.AddAsync(adjunto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EAdjunto adjunto)
        {
            _context.T_Adjuntos.Update(adjunto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var adjunto = await _context.T_Adjuntos.FindAsync(id);
            if (adjunto != null)
            {
                _context.T_Adjuntos.Remove(adjunto);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task AgregarAdjuntoAsync(EAdjunto adjunto)
        {
            _context.T_Adjuntos.Add(adjunto);
            await _context.SaveChangesAsync();
        }
        
        /*
        public async Task<IEnumerable<EAdjunto>> ObtenerPorTareaAsync(int idTarea)
        {
            return await _context.T_Adjuntos
                .Where(a => a.CN_Documento == idTarea)
                .ToListAsync();
        }*/

    }
}
