using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var permisos = await _repository.GetAllAsync();
            return Ok(permisos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var permiso = await _repository.GetByIdAsync(id);
            return permiso != null ? Ok(permiso) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EPermiso permiso)
        {
            // Obtener el último ID actual
            var permisos = await _repository.GetAllAsync();
            var ultimoId = permisos.Any() ? permisos.Max(p => p.CN_Id_permiso) : 0;

            // Asignar el nuevo ID sumando uno
            permiso.CN_Id_permiso = ultimoId + 1;


            // Guardar el permiso
            await _repository.AddAsync(permiso);

            return CreatedAtAction(nameof(Get), new { id = permiso.CN_Id_permiso }, permiso);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EPermiso permiso)
        {
            if (id != permiso.CN_Id_permiso)
                return BadRequest("El id no coincide.");

            await _repository.UpdateAsync(permiso);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("registrados")]
        public async Task<IActionResult> GetPermisosRegistrados()
        {
            try
            {
                var permisosRegistrados = await _repository.GetPermisosPorEstadoRegistradoAsync();

                if (permisosRegistrados == null || !permisosRegistrados.Any())
                {
                    return NotFound("No hay permisos en estado Registrado.");
                }

                return Ok(permisosRegistrados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al consultar permisos: {ex.Message}");
            }
        }

        [HttpGet("registrados/cantidad")]
        public async Task<IActionResult> GetCantidadPermisosRegistrados()
        {
            try
            {
                // Llamamos al repositorio para obtener la cantidad de permisos en estado "Registrado"
                var cantidadPermisosRegistrados = await _repository.GetCantidadPermisosPorEstadoRegistradoAsync();

                // Devolvemos la cantidad en un formato adecuado
                return Ok(new { cantidad = cantidadPermisosRegistrados });
            }
            catch (Exception ex)
            {
                // Manejo de errores en caso de excepciones
                return StatusCode(500, $"Error al obtener la cantidad de permisos: {ex.Message}");
            }
        }




    }
}
