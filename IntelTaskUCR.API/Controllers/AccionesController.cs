//using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IntelTaskUCR.Domain.Entities;  // Asegúrate de importar el espacio correcto

using System;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccionesController : ControllerBase
    {
        private readonly IAccionRepository _repository;

        public AccionesController(IAccionRepository repository)
        {
            _repository = repository;
        }

        // Obtener todas las acciones
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync(); // Obtiene todas las acciones
            return Ok(items);
        }

        // Obtener una acción por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _repository.GetByIdAsync(id); // Obtiene la acción por ID
            return item != null ? Ok(item) : NotFound(); // Si no se encuentra, devuelve NotFound
        }

        // Crear una nueva acción
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EAccion entity)
        {
            await _repository.AddAsync(entity);
            return CreatedAtAction(nameof(Get), new { id = entity.CN_Id_accion }, entity);
        }

        // Actualizar una acción existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EAccion entity)
        {
            if (id != entity.CN_Id_accion) // Verifica si el id de la URL coincide con el del cuerpo de la solicitud
                return BadRequest("El id de la acción no coincide.");

            var result = await _repository.UpdateAsync(entity); // Actualiza la acción en la base de datos
            if (!result)
                return NotFound("Acción no encontrada."); // Si la acción no se encuentra, devuelve NotFound

            return NoContent(); // Devuelve código 204 si la actualización es exitosa
        }

        // Eliminar una acción
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id); // Elimina la acción por su ID
            if (!result)
                return NotFound("Acción no encontrada."); // Si no se encuentra, devuelve NotFound

            return NoContent(); // Devuelve código 204 si la acción fue eliminada exitosamente
        }
    }
}
