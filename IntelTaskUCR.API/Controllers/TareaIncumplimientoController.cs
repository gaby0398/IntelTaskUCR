using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareaIncumplimientoController : ControllerBase
    {
        private readonly ITareaIncumplimientoRepository _repository;

        public TareaIncumplimientoController(ITareaIncumplimientoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null) return NotFound();

            return Ok(item); // ← más correcto y común para GET por ID
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ETareaIncumplimiento item)
        {
            await _repository.AddAsync(item);
            return CreatedAtAction(nameof(Get), new { id = item.CN_Id_tarea_incumplimiento }, item);

        }
        // Obtiene los incumplimientos por ID de tarea (no por ID de incumplimiento)
        [HttpGet("PorTarea/{idTarea}")]
        public async Task<IActionResult> ObtenerPorTarea(int idTarea)
        {
            var items = await _repository.ObtenerPorTarea(idTarea);
            return Ok(items);
        }


        /*
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ETareaIncumplimiento item)
        {
            await _repository.AddAsync(item);
            return CreatedAtAction(nameof(Get), new { id = item.CN_Id_tarea_incumplimiento }, item);
        }*/

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ETareaIncumplimiento item)
        {
            if (id != item.CN_Id_tarea_incumplimiento)
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
