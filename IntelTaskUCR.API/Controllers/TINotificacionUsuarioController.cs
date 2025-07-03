using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TINotificacionUsuarioController : ControllerBase
    {
        private readonly ITINotificacionUsuarioRepository _repository;
        //private readonly INotificacionRepository _notificacionRepository;
        public TINotificacionUsuarioController(
            ITINotificacionUsuarioRepository repository,
            INotificacionRepository notificacionRepository)
        {
            _repository = repository;
            _notificacionRepository = notificacionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{idNotificacion}/{idUsuario}")]
        public async Task<IActionResult> Get(int idNotificacion, int idUsuario)
        {
            var item = await _repository.GetByIdsAsync(idNotificacion, idUsuario);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TINotificacionUsuario item)
        {
            await _repository.AddAsync(item);
            return CreatedAtAction(nameof(Get), new { idNotificacion = item.CN_Id_notificacion, idUsuario = item.CN_Id_usuario }, item);
        }

        [HttpPut("{idNotificacion}/{idUsuario}")]
        public async Task<IActionResult> Update(int idNotificacion, int idUsuario, [FromBody] TINotificacionUsuario item)
        {
            if (idNotificacion != item.CN_Id_notificacion || idUsuario != item.CN_Id_usuario)
                return BadRequest("Las claves primarias no coinciden.");

            await _repository.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete("{idNotificacion}/{idUsuario}")]
        public async Task<IActionResult> Delete(int idNotificacion, int idUsuario)
        {
            await _repository.DeleteAsync(idNotificacion, idUsuario);
            return NoContent();
        }

        private readonly INotificacionRepository _notificacionRepository;

       

        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> GetNotificacionesPorUsuario(int idUsuario)
        {
            // Paso 1: Obtener relaciones entre usuario y notificaciones
            var relaciones = await _repository.GetAllAsync();
            var idsNotificacionesUsuario = relaciones
                .Where(x => x.CN_Id_usuario == idUsuario)
                .Select(x => x.CN_Id_notificacion)
                .ToList();

            // Paso 2: Obtener todas las notificaciones y filtrar las que le pertenecen
            var todasLasNotificaciones = await _notificacionRepository.GetAllAsync();
            var notificacionesDelUsuario = todasLasNotificaciones
                .Where(n => idsNotificacionesUsuario.Contains(n.CN_Id_notificacion))
                .ToList();

            return Ok(notificacionesDelUsuario);
        }

    }
}
