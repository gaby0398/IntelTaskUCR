using IntelTaskUCR.API.Models.Usuarios;
using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;

        public UsuariosController(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        // Obtener todos los usuarios
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _repository.GetAllAsync();
            if (usuarios == null || !usuarios.Any())
            {
                return NoContent();
            }
            return Ok(usuarios);
        }

        // Obtener un usuario por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            return Ok(usuario);
        }

        // Crear un nuevo usuario
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EUsuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest("Datos incorrectos.");
            }

            var createdUsuario = await _repository.AddAsync(usuario);
            if (createdUsuario == null)
            {
                return StatusCode(500, "Hubo un problema al crear el usuario.");
            }

            return CreatedAtAction(nameof(Get), new { id = createdUsuario.CN_Id_usuario }, createdUsuario);
        }

        // Actualizar un usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EUsuario usuario)
        {
            var result = await _repository.UpdateAsync(usuario);
            if (!result)
            {
                return NotFound("Usuario no encontrado.");
            }

            return NoContent();
        }

        // Eliminar un usuario
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
            {
                return NotFound("Usuario no encontrado.");
            }

            return NoContent();
        }
        // En UsuarioController.cs
        [HttpGet("usuarios-activos")]
        public async Task<IActionResult> GetUsuariosActivos()
        {
            var usuariosActivos = await _repository.GetUsuariosActivosAsync();
            return Ok(new { usuariosActivos });
        }


        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] string estado)
        {
            // Buscar al usuario por su ID
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Obtener el usuario actual desde el contexto de la solicitud (JWT u otro sistema de autenticación)
            var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == "IdRol")?.Value;

            // Verificar si el usuario actual tiene el rol adecuado (por ejemplo, rol 1 = administrador)
            if (currentUserRole != "1")
            {
                return Unauthorized("No tiene permiso para cambiar el estado de este usuario.");
            }

            // Validación del estado
            if (estado.ToLower() != "activo" && estado.ToLower() != "inactivo")
            {
                return BadRequest("Estado inválido. Debe ser 'activo' o 'inactivo'.");
            }

            // Cambiar el estado del usuario
            usuario.CB_Estado_usuario = estado.ToLower() == "activo";

            // Actualizar el usuario en la base de datos
            await _repository.UpdateAsync(usuario);

            // Responder con éxito
            return Ok(new { message = "Estado actualizado correctamente", usuario });
        }



        [Authorize]
        [HttpPut("{id}/CambiarPassword")]
        public async Task<IActionResult> CambiarPassword(int id, [FromBody] CambiarPasswordDTO dto)
        {
            try
            {
                var resultado = await _repository.CambiarPasswordAsync(id, dto.ContrasennaActual, dto.NuevaContrasenna);
                if (!resultado)
                    return NotFound("Usuario no encontrado.");

                return Ok("Contraseña actualizada exitosamente.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Filtrar")]
        public async Task<IActionResult> FiltrarUsuarios([FromQuery] string? nombre, [FromQuery] int? rolId, [FromQuery] int? oficinaId)
        {
            var usuarios = await _repository.FiltrarUsuarios(nombre, rolId, oficinaId);
            return Ok(usuarios);
        }






    }
}
