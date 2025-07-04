using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermisoController : ControllerBase
    {
        private readonly IPermisoRepository _repository;

        public PermisoController(IPermisoRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Permiso
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var permisos = await _repository.GetAllAsync();
            return Ok(permisos);
        }

        // GET: api/Permiso/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EPermiso>> GetById(int id)
        {
            var permiso = await _repository.GetByIdAsync(id);
            if (permiso == null)
                return NotFound();

            return Ok(permiso);
        }

        // POST: api/Permiso
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EPermiso permiso)
        {
            var permisos = await _repository.GetAllAsync();
            var ultimoId = permisos.Any() ? permisos.Max(p => p.CN_Id_permiso) : 0;
            permiso.CN_Id_permiso = ultimoId + 1;

            await _repository.AddAsync(permiso);
            return CreatedAtAction(nameof(GetById), new { id = permiso.CN_Id_permiso }, permiso);
        }

        // PUT: api/Permiso/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EPermiso permiso)
        {
            if (id != permiso.CN_Id_permiso)
                return BadRequest("El id no coincide.");

            await _repository.UpdateAsync(permiso);
            return NoContent();
        }

        // DELETE: api/Permiso/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        // GET: api/Permiso/registrados
        [HttpGet("registrados")]
        public async Task<IActionResult> GetPermisosRegistrados()
        {
            try
            {
                var permisosRegistrados = await _repository.GetPermisosPorEstadoRegistradoAsync();
                if (permisosRegistrados == null || !permisosRegistrados.Any())
                    return NotFound("No hay permisos en estado Registrado.");

                return Ok(permisosRegistrados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al consultar permisos: {ex.Message}");
            }
        }

        // GET: api/Permiso/registrados/cantidad
        [HttpGet("registrados/cantidad")]
        public async Task<IActionResult> GetCantidadPermisosRegistrados()
        {
            try
            {
                var cantidad = await _repository.GetCantidadPermisosPorEstadoRegistradoAsync();
                return Ok(new { cantidad });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la cantidad de permisos: {ex.Message}");
            }
        }

        // PATCH: api/Permiso/{id}/estado
        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] byte nuevoEstado)
        {
            var permiso = await _repository.GetByIdAsync(id);
            if (permiso == null)
                return NotFound();

            permiso.CN_Id_estado = nuevoEstado;
            await _repository.UpdateAsync(permiso);

            return Ok(new { mensaje = "Estado actualizado correctamente." });
        }
    }
}
