using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class EstadoRepository : IEstadoRepository
    {
        private readonly IntelTaskDbContext _context;

        // Constructor
        public EstadoRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        // Obtener todos los estados
        public async Task<IEnumerable<EEstado>> GetAllAsync()
        {
            return await _context.T_Estados.ToListAsync(); // Obtener todos los estados
        }

        // Obtener un estado por ID
        public async Task<EEstado?> GetByIdAsync(int id)
        {
            return await _context.T_Estados.FindAsync(id); // Buscar el estado por su ID
        }

        // Agregar un nuevo estado
        public async Task AddAsync(EEstado estado)
        {
            await _context.T_Estados.AddAsync(estado); // Añadir el estado a la base de datos
            await _context.SaveChangesAsync(); // Guardar los cambios
        }

        // Actualizar un estado existente
        public async Task UpdateAsync(EEstado estado)
        {
            _context.T_Estados.Update(estado); // Actualizar el estado en la base de datos
            await _context.SaveChangesAsync(); // Guardar los cambios
        }

        // Eliminar un estado por su ID
        public async Task DeleteAsync(int id)
        {
            var entidad = await _context.T_Estados.FindAsync(id); // Buscar el estado por su ID

            if (entidad != null)
            {
                _context.T_Estados.Remove(entidad); // Eliminar el estado
                await _context.SaveChangesAsync(); // Guardar los cambios
            }
        }
    }
}
