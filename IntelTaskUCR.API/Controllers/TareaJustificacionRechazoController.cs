using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareaJustificacionRechazoController : ControllerBase
    {
        private readonly ITareaJustificacionRechazoRepository _repository;

        public TareaJustificacionRechazoController(ITareaJustificacionRechazoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lista = await _repository.GetAllAsync();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item != null ? Ok(item) : NotFound();
        }


        [HttpGet("tarea/{tareaId}")]
        public async Task<IActionResult> GetByTareaId(int tareaId)
        {
            var justificaciones = await _repository.GetByTareaIdAsync(tareaId);
            if (justificaciones == null || !justificaciones.Any())
                return NotFound();

            return Ok(justificaciones);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ETareaJustificacionRechazo item)
        {
            await _repository.AddAsync(item);
            return CreatedAtAction(nameof(Get), new { id = item.CN_Id_tarea_rechazo }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ETareaJustificacionRechazo item)
        {
            if (id != item.CN_Id_tarea_rechazo)
                return BadRequest("El ID no coincide.");

            await _repository.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }


    }
}
