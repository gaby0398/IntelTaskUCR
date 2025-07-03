using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BitacoraCambioEstadoController : ControllerBase
    {
        private readonly IBitacoraCambioEstadoRepository _repository;

        public BitacoraCambioEstadoController(IBitacoraCambioEstadoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cambios = await _repository.GetAllAsync();
            return Ok(cambios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cambio = await _repository.GetByIdAsync(id);
            return cambio != null ? Ok(cambio) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EBitacoraCambioEstado cambio)
        {
            await _repository.AddAsync(cambio);
            return CreatedAtAction(nameof(Get), new { id = cambio.CN_Id_cambio_estado }, cambio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EBitacoraCambioEstado cambio)
        {
            if (id != cambio.CN_Id_cambio_estado)
                return BadRequest("El ID no coincide.");

            await _repository.UpdateAsync(cambio);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("PorTarea/{tareaId}")]
        public async Task<IActionResult> GetPorTarea(int tareaId)
        {
            var cambios = await _repository.GetByTareaIdAsync(tareaId);
            return Ok(cambios);
        }

    }
}
