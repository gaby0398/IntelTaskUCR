using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrioridadesController : ControllerBase
    {
        private readonly IPrioridadRepository _repository;

        public PrioridadesController(IPrioridadRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var prioridades = await _repository.GetAllAsync();
            return Ok(prioridades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(byte id)
        {
            var prioridad = await _repository.GetByIdAsync(id);
            return prioridad != null ? Ok(prioridad) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EPrioridad prioridad)
        {
            await _repository.AddAsync(prioridad);
            return CreatedAtAction(nameof(Get), new { id = prioridad.CN_Id_prioridad }, prioridad);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(byte id, [FromBody] EPrioridad prioridad)
        {
            if (id != prioridad.CN_Id_prioridad)
                return BadRequest("El id no coincide.");

            await _repository.UpdateAsync(prioridad);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(byte id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
