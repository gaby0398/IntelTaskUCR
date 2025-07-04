using IntelTaskUCR.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IAccionRepository
    {
        // Obtener todas las acciones
        Task<IEnumerable<EAccion>> GetAllAsync();

        // Obtener una acción por ID
        Task<EAccion?> GetByIdAsync(int id);

        // Añadir una nueva acción
        Task AddAsync(EAccion accion);

        // Actualizar una acción existente
        Task<bool> UpdateAsync(EAccion accion);

        // Eliminar una acción por su ID
        Task<bool> DeleteAsync(int id);
    }
}
