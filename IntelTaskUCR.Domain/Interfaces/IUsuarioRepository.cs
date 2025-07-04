using IntelTaskUCR.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntelTaskUCR.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<EUsuario>> GetAllAsync(); // Obtiene todos los usuarios
        Task<EUsuario> GetByIdAsync(int id); // Obtiene un usuario por ID
        Task<EUsuario> AddAsync(EUsuario usuario); // Añade un nuevo usuario
        Task<bool> UpdateAsync(EUsuario usuario); // Actualiza un usuario
        Task<bool> DeleteAsync(int id); // Elimina un usuario

        Task<EUsuario?> GetByCorreoAsync(string correo);

        Task<bool> CambiarPasswordAsync(int idUsuario, string contrasennaActual, string nuevaContrasenna);

        Task<int> GetUsuariosActivosAsync();// Método para contar usuarios activos

        Task<IEnumerable<EUsuario>> FiltrarUsuarios(string? nombre, int? rolId, int? oficinaId);

    }



}
