using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplejidadController : ControllerBase
    {
        private readonly IComplejidadRepository _repository;

        public ComplejidadController(IComplejidadRepository repository)
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
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EComplejidad item)
        {
            await _repository.AddAsync(item);
            return CreatedAtAction(nameof(Get), new { id = item.CN_Id_complejidad }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EComplejidad item)
        {
            if (id != item.CN_Id_complejidad)
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
