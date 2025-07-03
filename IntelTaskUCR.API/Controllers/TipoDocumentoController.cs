using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoDocumentoController : ControllerBase
    {
        private readonly ITipoDocumentoRepository _repository;

        public TipoDocumentoController(ITipoDocumentoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tipos = await _repository.GetAllAsync();
            return Ok(tipos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var tipo = await _repository.GetByIdAsync(id);
            return tipo != null ? Ok(tipo) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ETipoDocumento tipo)
        {
            await _repository.AddAsync(tipo);
            return CreatedAtAction(nameof(Get), new { id = tipo.CN_Id_tipo_documento }, tipo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ETipoDocumento tipo)
        {
            if (id != tipo.CN_Id_tipo_documento)
                return BadRequest("El ID no coincide.");

            await _repository.UpdateAsync(tipo);
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
