using IntelTaskUCR.Domain.Entities;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface ITareaRepository
    {
        Task<IEnumerable<ETarea>> GetAllAsync();
        Task<ETarea?> GetByIdAsync(int id);
        Task AddAsync(ETarea tarea);
        Task UpdateAsync(ETarea tarea);
        Task DeleteAsync(int id);

        Task<IEnumerable<ETarea>> GetTareasPorEstadoAsync(byte estadoId);

        Task<int> GetCantidadTareasProximasAVencerAsync();

        Task<IEnumerable<ETarea>> FiltrarTareasAsync(string? nombre, int? estado, int? prioridad, string? asignado, DateTime? fechaLimite);

        Task<IEnumerable<ETarea>> GetTareasPorUsuarioAsync(int idUsuario);

    }
}
