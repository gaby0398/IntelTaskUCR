using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OficinaController : ControllerBase
    {
        private readonly IOficinaRepository _repository;

        public OficinaController(IOficinaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var oficinas = await _repository.GetAllAsync();
            return Ok(oficinas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var oficina = await _repository.GetByIdAsync(id);
            return oficina != null ? Ok(oficina) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EOficina oficina)
        {
            await _repository.AddAsync(oficina);
            return CreatedAtAction(nameof(Get), new { id = oficina.CN_Codigo_oficina }, oficina);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EOficina oficina)
        {
            if (id != oficina.CN_Codigo_oficina)
                return BadRequest("El id no coincide.");

            await _repository.UpdateAsync(oficina);
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
