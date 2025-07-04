using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRolesRepository _repository;

        public RolesController(IRolesRepository repository)
        {
            _repository = repository;
        }

        // Obtener todos los roles
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _repository.GetAllAsync();
            return Ok(roles);
        }

        // Obtener un rol por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var role = await _repository.GetByIdAsync(id);
            if (role == null)
                return NotFound("Rol no encontrado.");
            return Ok(role);
        }

        // Crear un nuevo rol
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ERoles role)
        {
            if (role == null)
                return BadRequest("Datos del rol incorrectos.");
            await _repository.AddAsync(role);
            return CreatedAtAction(nameof(Get), new { id = role.CN_Id_rol }, role);
        }

        // Actualizar un rol
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ERoles role)
        {
            if (id != role.CN_Id_rol)
                return BadRequest("El id del rol no coincide.");
            await _repository.UpdateAsync(role);
            return NoContent();
        }

        // Eliminar un rol
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
