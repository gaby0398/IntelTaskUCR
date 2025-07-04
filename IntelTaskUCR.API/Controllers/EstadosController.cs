using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadosController : Controller
    {
        private readonly IEstadoRepository _repository;
        public EstadosController(IEstadoRepository repository) { _repository = repository; }

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
        public async Task<IActionResult> Create([FromBody] EEstado entity)
        {
            await _repository.AddAsync(entity);
            return CreatedAtAction(nameof(Get), new { id = entity.CN_Id_estado }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Updtate(int id, [FromBody] EEstado entity)
        {

            if (id != entity.CN_Id_estado)
                return BadRequest("El id está vacio.");

            await _repository.UpdateAsync(entity);
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
