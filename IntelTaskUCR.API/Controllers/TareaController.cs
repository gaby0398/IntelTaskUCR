using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
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
        public async Task<IActionResult> Create([FromBody] ETarea tarea)
        {
            if (string.IsNullOrWhiteSpace(tarea.CT_Titulo_tarea))
                return BadRequest("El título de la tarea es obligatorio.");

            if (string.IsNullOrWhiteSpace(tarea.CT_Descripcion_tarea))
                return BadRequest("La descripción de la tarea es obligatoria.");

            if (tarea.CN_Id_prioridad <= 0 || tarea.CN_Id_complejidad <= 0)
                return BadRequest("Prioridad y complejidad son campos obligatorios.");

            if (tarea.CF_Fecha_limite <= tarea.CF_Fecha_asignacion)
                return BadRequest("La fecha límite debe ser posterior a la fecha de asignación.");

            var userIdClaim = User.FindFirst("IdUsuario");
            if (userIdClaim == null) return Unauthorized("No se pudo identificar al usuario.");
            int userId = int.Parse(userIdClaim.Value);

            tarea.CN_Usuario_creador = userId;

            await _repository.AddAsync(tarea);

            await _bitacoraAccionRepository.AddAsync(new EBitacoraAccion
            {
                CN_Id_usuario = userId,
                CN_Id_accion = 1, // Crear
                CN_Id_pantalla = 2, // Tareas
                CN_Id_tipo_documento = 1, // Tipo documento: Tarea
                CF_Fecha_hora_registro = DateTime.Now,
                CT_informacion_importante = $"Se creó la tarea '{tarea.CT_Titulo_tarea}'."
            });

            return CreatedAtAction(nameof(Get), new { id = tarea.CN_Id_tarea }, tarea);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ETarea tarea)
        {
            try
            {
                if (id != tarea.CN_Id_tarea)
                    return BadRequest("El ID no coincide con la tarea.");

                if (string.IsNullOrWhiteSpace(tarea.CT_Titulo_tarea))
                    return BadRequest("El título de la tarea es obligatorio.");

                if (string.IsNullOrWhiteSpace(tarea.CT_Descripcion_tarea))
                    return BadRequest("La descripción de la tarea es obligatoria.");

                if (tarea.CN_Id_prioridad <= 0 || tarea.CN_Id_estado <= 0 || tarea.CN_Id_complejidad <= 0)
                    return BadRequest("Prioridad, estado y complejidad son campos obligatorios.");

                if (tarea.CF_Fecha_limite <= tarea.CF_Fecha_asignacion)
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

                await _repository.UpdateAsync(tarea);

                await _bitacoraAccionRepository.AddAsync(new EBitacoraAccion
                {
                    CN_Id_usuario = userId,
                    CN_Id_accion = 3,
                    CN_Id_pantalla = 2,
                    CN_Id_tipo_documento = 1,
                    CF_Fecha_hora_registro = DateTime.Now,
                    CT_informacion_importante = $"Se actualizó la tarea con ID {tarea.CN_Id_tarea}."
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

    }
}
