using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TareaController : ControllerBase
    {
        private readonly ITareaRepository _repository;
        private readonly IBitacoraAccionRepository _bitacoraAccionRepository;

        public TareaController(ITareaRepository repository, IBitacoraAccionRepository bitacoraAccionRepository)
        {
            _repository = repository;
            _bitacoraAccionRepository = bitacoraAccionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tareas = await _repository.GetAllAsync();
            return Ok(tareas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var tarea = await _repository.GetByIdAsync(id);
            return tarea != null ? Ok(tarea) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearTareaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CT_Titulo_tarea))
                return BadRequest("El título de la tarea es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.CT_Descripcion_tarea))
                return BadRequest("La descripción de la tarea es obligatoria.");

            if (dto.CN_Id_prioridad <= 0 || dto.CN_Id_complejidad <= 0)
                return BadRequest("Prioridad y complejidad son campos obligatorios.");

            // Validar fechas
            if (dto.CF_Fecha_limite <= dto.CF_Fecha_asignacion)
                return BadRequest("La fecha límite debe ser posterior a la fecha de asignación.");

            // Obtener el usuario autenticado
            var userIdClaim = User.FindFirst("IdUsuario");
            if (userIdClaim == null)
                return Unauthorized("No se pudo identificar al usuario.");

            int userId = int.Parse(userIdClaim.Value);

            // Crear nueva tarea a partir del DTO
            var nuevaTarea = new ETarea
            {
                CN_Tarea_origen = dto.CN_Tarea_origen,
                CT_Titulo_tarea = dto.CT_Titulo_tarea,
                CT_Descripcion_tarea = dto.CT_Descripcion_tarea,
                CT_Descripcion_espera = string.IsNullOrWhiteSpace(dto.CT_Descripcion_espera) ? " " : dto.CT_Descripcion_espera,
                CN_Id_complejidad = dto.CN_Id_complejidad,
                CN_Id_estado = dto.CN_Id_estado ?? 1, // Si viene null, se asigna estado por defecto
                CN_Id_prioridad = dto.CN_Id_prioridad,
                CN_Numero_GIS = string.IsNullOrWhiteSpace(dto.CN_Numero_GIS) ? " " : dto.CN_Numero_GIS,
                CF_Fecha_asignacion = dto.CF_Fecha_asignacion,
                CF_Fecha_limite = dto.CF_Fecha_limite,
                CF_Fecha_finalizacion = dto.CF_Fecha_finalizacion,
                CN_Usuario_creador = userId,
                CN_Usuario_asignado = dto.CN_Usuario_asignado
            };

            // Guardar en BD
            await _repository.AddAsync(nuevaTarea);

            // Bitácora
            await _bitacoraAccionRepository.AddAsync(new EBitacoraAccion
            {
                CN_Id_usuario = userId,
                CN_Id_accion = 1,
                CN_Id_pantalla = 2,
                CN_Id_tipo_documento = 1,
                CF_Fecha_hora_registro = DateTime.Now,
                CT_informacion_importante = $"Se creó la tarea '{dto.CT_Titulo_tarea}'."
            });

            return CreatedAtAction(nameof(Get), new { id = nuevaTarea.CN_Id_tarea }, nuevaTarea);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CrearTareaDto dto)
        {
            try
            {
                if (id != dto.CN_Id_tarea)
                    return BadRequest("El ID no coincide con la tarea.");

                if (string.IsNullOrWhiteSpace(dto.CT_Titulo_tarea))
                    return BadRequest("El título de la tarea es obligatorio.");

                if (string.IsNullOrWhiteSpace(dto.CT_Descripcion_tarea))
                    return BadRequest("La descripción de la tarea es obligatoria.");

                if (dto.CN_Id_prioridad <= 0 || dto.CN_Id_estado <= 0 || dto.CN_Id_complejidad <= 0)
                    return BadRequest("Prioridad, estado y complejidad son campos obligatorios.");

                if (dto.CF_Fecha_limite <= dto.CF_Fecha_asignacion)
                    return BadRequest("La fecha límite debe ser posterior a la fecha de asignación.");

                var userIdClaim = User.FindFirst("IdUsuario");
                if (userIdClaim == null)
                    return Unauthorized("No se pudo identificar al usuario.");
                int userId = int.Parse(userIdClaim.Value);

                var tareaOriginal = await _repository.GetByIdAsync(id);
                if (tareaOriginal == null)
                    return NotFound("La tarea no existe.");

                // ✅ Validar usuario creador o asignado
                if (tareaOriginal.CN_Usuario_creador != userId && tareaOriginal.CN_Usuario_asignado != userId)
                    return Forbid("Solo el creador o el asignado pueden modificar esta tarea.");

                await _repository.UpdateAsync(tareaOriginal);

                await _bitacoraAccionRepository.AddAsync(new EBitacoraAccion
                {
                    CN_Id_usuario = userId,
                    CN_Id_accion = 3,
                    CN_Id_pantalla = 2,
                    CN_Id_tipo_documento = 1,
                    CF_Fecha_hora_registro = DateTime.Now,
                    CT_informacion_importante = $"Se actualizó la tarea con ID {dto.CN_Id_tarea}."
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error interno: {ex.Message}");
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("filtrada")]
        public async Task<IActionResult> FiltrarPorId([FromQuery] int cN_Id_tarea)
        {
            var tarea = await _repository.GetByIdAsync(cN_Id_tarea);
            return tarea != null ? Ok(tarea) : NotFound();
        }

        [HttpGet("enProceso")]
        public async Task<IActionResult> GetTareasEnProceso()
        {
            var tareasEnProceso = await _repository.GetTareasPorEstadoAsync(3);
            if (tareasEnProceso == null || !tareasEnProceso.Any())
                return NotFound("No hay tareas en proceso.");

            return Ok(new { cantidad = tareasEnProceso.Count() });
        }

        [HttpGet("proximas-a-vencer/cantidad")]
        public async Task<IActionResult> GetCantidadTareasProximasAVencer()
        {
            try
            {
                var cantidadTareasProximasAVencer = await _repository.GetCantidadTareasProximasAVencerAsync();
                return Ok(new { cantidad = cantidadTareasProximasAVencer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la cantidad de tareas próximas a vencer: {ex.Message}");
            }
        }

        [HttpGet("filtrar-por-usuario")]
        public async Task<IActionResult> FiltrarPorNombreDeUsuario([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("Debes indicar un nombre de usuario para filtrar.");

            var tareas = await _repository.GetAllAsync();

            // Este filtro requiere que hayas hecho Include en el repositorio
            var filtradas = tareas
                .Where(t =>
                    (t.UsuarioCreador != null && t.UsuarioCreador.CT_Nombre_usuario.Contains(nombre, StringComparison.OrdinalIgnoreCase)) ||
                    (t.UsuarioAsignado != null && t.UsuarioAsignado.CT_Nombre_usuario.Contains(nombre, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return filtradas.Any() ? Ok(filtradas) : NotFound("No se encontraron tareas con ese usuario.");
        }


        [HttpGet("FiltrarTareas")]
        public async Task<IActionResult> FiltrarTareas(
        [FromQuery] string? nombre,
        [FromQuery] int? estado,
        [FromQuery] int? prioridad,
        [FromQuery] string? asignado,
        [FromQuery] DateTime? fechaLimite)
        {
            var tareas = await _repository.FiltrarTareasAsync(nombre, estado, prioridad, asignado, fechaLimite);
            return Ok(tareas);
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> GetTareasPorUsuario(int idUsuario)
        {
            var tareas = await _repository.GetTareasPorUsuarioAsync(idUsuario);
            return Ok(tareas);
        }


    }
}
