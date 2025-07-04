using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IEstadoRepository
    {
        // Obtener todos los estados
        Task<IEnumerable<EEstado>> GetAllAsync();

        // Obtener un estado por ID
        Task<EEstado?> GetByIdAsync(int id);

        // Agregar un nuevo estado
        Task AddAsync(EEstado estado);

        // Actualizar un estado existente
        Task UpdateAsync(EEstado estado);

        // Eliminar un estado por ID
        Task DeleteAsync(int id);
    }
}
