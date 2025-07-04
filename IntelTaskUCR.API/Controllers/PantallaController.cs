using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PantallaController : ControllerBase
    {
        private readonly IPantallaRepository _repository;

        public PantallaController(IPantallaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pantallas = await _repository.GetAllAsync();
            return Ok(pantallas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var pantalla = await _repository.GetByIdAsync(id);
            return pantalla != null ? Ok(pantalla) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EPantalla pantalla)
        {
            await _repository.AddAsync(pantalla);
            return CreatedAtAction(nameof(Get), new { id = pantalla.CN_Id_pantalla }, pantalla);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EPantalla pantalla)
        {
            if (id != pantalla.CN_Id_pantalla)
                return BadRequest("El id no coincide.");

            await _repository.UpdateAsync(pantalla);
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
