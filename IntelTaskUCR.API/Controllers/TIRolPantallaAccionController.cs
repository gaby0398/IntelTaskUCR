using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TIRolPantallaAccionController : ControllerBase
    {
        private readonly ITIRolPantallaAccionRepository _repository;

        public TIRolPantallaAccionController(ITIRolPantallaAccionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{idPantalla}/{idAccion}/{idRol}")]
        public async Task<IActionResult> Get(int idPantalla, int idAccion, int idRol)
        {
            var item = await _repository.GetByIdsAsync(idPantalla, idAccion, idRol);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TIRolPantallaAccion item)
        {
            await _repository.AddAsync(item);
            return CreatedAtAction(nameof(Get), new
            {
                idPantalla = item.CN_Id_pantalla,
                idAccion = item.CN_Id_accion,
                idRol = item.CN_Id_rol
            }, item);
        }

        [HttpDelete("{idPantalla}/{idAccion}/{idRol}")]
        public async Task<IActionResult> Delete(int idPantalla, int idAccion, int idRol)
        {
            await _repository.DeleteAsync(idPantalla, idAccion, idRol);
            return NoContent();
        }
    }
}
