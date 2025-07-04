using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionController : ControllerBase
    {
        private readonly INotificacionRepository _repository;

        public NotificacionController(INotificacionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notificaciones = await _repository.GetAllAsync();
            return Ok(notificaciones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var notificacion = await _repository.GetByIdAsync(id);
            return notificacion != null ? Ok(notificacion) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ENotificacion notificacion)
        {
            await _repository.AddAsync(notificacion);
            return CreatedAtAction(nameof(Get), new { id = notificacion.CN_Id_notificacion }, notificacion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ENotificacion notificacion)
        {
            if (id != notificacion.CN_Id_notificacion)
                return BadRequest("El id no coincide.");

            await _repository.UpdateAsync(notificacion);
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
