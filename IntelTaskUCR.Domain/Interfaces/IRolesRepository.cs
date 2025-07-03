using IntelTaskUCR.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IRolesRepository
    {
        // Obtener todos los roles
        Task<IEnumerable<ERoles>> GetAllAsync();

        // Obtener un rol por su ID
        Task<ERoles?> GetByIdAsync(int id);

        // Agregar un nuevo rol
        Task AddAsync(ERoles role);

        // Actualizar un rol existente
        Task UpdateAsync(ERoles role);

        // Eliminar un rol por su ID
        Task DeleteAsync(int id);
    }
}
