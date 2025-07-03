using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly IntelTaskDbContext _context;

        // Constructor de la clase que recibe el contexto de la base de datos
        public RolesRepository(IntelTaskDbContext context) { _context = context; }

        // Método para obtener todos los roles
        public async Task<IEnumerable<ERoles>> GetAllAsync()
        {
            return await _context.T_Roles.ToListAsync(); // Obtener todos los roles desde la tabla T_Roles
        }

        // Método para obtener un rol por su ID
        public async Task<ERoles?> GetByIdAsync(int id)
        {
            return await _context.T_Roles.FindAsync(id); // Buscar el rol por ID en la tabla T_Roles
        }

        // Método para agregar un nuevo rol
        public async Task AddAsync(ERoles role)
        {
            await _context.T_Roles.AddAsync(role); // Agregar el rol a la tabla T_Roles
            await _context.SaveChangesAsync(); // Guardar los cambios en la base de datos
        }

        // Método para actualizar un rol existente
        public async Task UpdateAsync(ERoles role)
        {
            _context.T_Roles.Update(role); // Actualizar el rol en la tabla T_Roles
            await _context.SaveChangesAsync(); // Guardar los cambios en la base de datos
        }

        // Método para eliminar un rol por su ID
        public async Task DeleteAsync(int id)
        {
            var entidad = await _context.T_Roles.FindAsync(id); // Buscar el rol por ID

            if (entidad != null)
            {
                _context.T_Roles.Remove(entidad); // Eliminar el rol de la tabla T_Roles
                await _context.SaveChangesAsync(); // Guardar los cambios en la base de datos
            }
        }
    }
}
