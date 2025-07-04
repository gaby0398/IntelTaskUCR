using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt.Net;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IntelTaskDbContext _context;

        public UsuarioRepository(IntelTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EUsuario>> GetAllAsync()
        {
            return await _context.T_Usuarios.ToListAsync();
        }

        public async Task<EUsuario> GetByIdAsync(int id)
        {
            return await _context.T_Usuarios.FindAsync(id);
        }

        public async Task<EUsuario> AddAsync(EUsuario usuario)
        {
            // Obtener el último ID de la base de datos
            var ultimoId = await ObtenerUltimoIdUsuarioAsync();

            // Asignar el nuevo ID al usuario (sumamos 1 al último ID)
            usuario.CN_Id_usuario = ultimoId + 1;

            // Cifrar la contraseña antes de guardarla
            usuario.CT_Contrasenna = BCrypt.Net.BCrypt.HashPassword(usuario.CT_Contrasenna);

            // Guardamos el nuevo usuario
            await _context.T_Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return usuario;  // Devuelve el usuario creado con el ID asignado
        }


        public async Task<bool> UpdateAsync(EUsuario usuario)
        {
            // Si la contraseña es nueva, se debe cifrar
            if (!string.IsNullOrEmpty(usuario.CT_Contrasenna))
            {
                usuario.CT_Contrasenna = BCrypt.Net.BCrypt.HashPassword(usuario.CT_Contrasenna);
            }

            _context.T_Usuarios.Update(usuario);
            var result = await _context.SaveChangesAsync();
            return result > 0; // Si la actualización fue exitosa, devuelve true
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.T_Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            _context.T_Usuarios.Remove(usuario);
            var result = await _context.SaveChangesAsync();
            return result > 0; // Si el usuario fue eliminado, devuelve true
        }

        public async Task<EUsuario?> GetByCorreoAsync(string correo)
        {
            return await _context.T_Usuarios
                .FirstOrDefaultAsync(u => u.CT_Correo_usuario == correo);
        }

        public async Task<int> GetUsuariosActivosAsync()
        {
            return await _context.T_Usuarios.CountAsync(u => u.CB_Estado_usuario == true);
        }

        public async Task<int> ObtenerUltimoIdUsuarioAsync()
        {
            // Obtener el último CN_Id_usuario de la tabla T_Usuarios
            var ultimoId = await _context.T_Usuarios
                                         .OrderByDescending(u => u.CN_Id_usuario)
                                         .Select(u => u.CN_Id_usuario)
                                         .FirstOrDefaultAsync();

            return ultimoId;
        }

        public async Task<bool> CambiarPasswordAsync(int idUsuario, string contrasennaActual, string nuevaContrasenna)
        {
            var usuario = await _context.T_Usuarios.FindAsync(idUsuario);
            if (usuario == null)
                return false;

            // Verificar contraseña actual
            if (!BCrypt.Net.BCrypt.Verify(contrasennaActual, usuario.CT_Contrasenna))
                throw new InvalidOperationException("La contraseña actual es incorrecta.");

            // Encriptar nueva contraseña
            usuario.CT_Contrasenna = BCrypt.Net.BCrypt.HashPassword(nuevaContrasenna);

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<EUsuario>> FiltrarUsuarios(string? nombre, int? rolId, int? oficinaId)
        {
            var query = _context.T_Usuarios.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(u => u.CT_Nombre_usuario.Contains(nombre));
            }

            if (rolId.HasValue)
            {
                query = query.Where(u => u.CN_Id_rol == rolId);
            }

            if (oficinaId.HasValue)
            {
                query = from u in query
                        join tu in _context.TI_Usuario_X_Oficina
                        on u.CN_Id_usuario equals tu.CN_Id_usuario
                        where tu.CN_Codigo_oficina == oficinaId.Value
                        select u;
            }

            return await query.ToListAsync();
        }



    }
}
