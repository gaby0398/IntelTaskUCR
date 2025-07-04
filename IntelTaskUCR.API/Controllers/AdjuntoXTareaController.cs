using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdjuntoXTareaController : ControllerBase
    {
        private readonly IAdjuntoXTareaRepository _repository;
        private readonly IntelTaskDbContext _context;

        public AdjuntoXTareaController(IAdjuntoXTareaRepository repository, IntelTaskDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("tarea/{id}")]
        public async Task<IActionResult> GetByTarea(int id)
        {
            var result = await _repository.GetByTareaIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EAdjuntoXTarea entity)
        {
            try
            {
                await _repository.AddAsync(entity);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error al guardar la relación: {ex.Message}");
            }
        }

        [HttpDelete("{idTarea}/{idAdjunto}")]
        public async Task<IActionResult> Delete(int idTarea, int idAdjunto)
        {
            await _repository.DeleteAsync(idTarea, idAdjunto);
            return NoContent();
        }

        [HttpGet("tarea/{id}/archivos")]
        public async Task<IActionResult> GetAdjuntosByTarea(int id)
        {
            var relaciones = await _repository.GetByTareaIdAsync(id);

            if (relaciones == null || !relaciones.Any())
            {
                return Ok(new List<object>());
            }

            var idsAdjuntos = relaciones
                .Where(r => r != null)
                .Select(r => r.CN_Id_adjuntos)
                .ToList();

            if (!idsAdjuntos.Any())
            {
                return Ok(new List<object>());
            }

            var adjuntos = await _context.T_Adjuntos
                .Where(a => idsAdjuntos.Contains(a.CN_Id_adjuntos))
                .ToListAsync();

            return Ok(adjuntos);
        }
    }
}
