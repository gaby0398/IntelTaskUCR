using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdjuntoController : ControllerBase
    {
        private readonly IAdjuntoRepository _repository;
        private readonly IWebHostEnvironment _env;

        public AdjuntoController(IAdjuntoRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var adjuntos = await _repository.GetAllAsync();
            return Ok(adjuntos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var adjunto = await _repository.GetByIdAsync(id);
            return adjunto != null ? Ok(adjunto) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EAdjunto adjunto)
        {
            try
            {
                await _repository.AddAsync(adjunto);
                return Ok(adjunto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error al guardar el adjunto: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EAdjunto adjunto)
        {
            if (id != adjunto.CN_Id_adjuntos)
                return BadRequest("El ID no coincide.");

            await _repository.UpdateAsync(adjunto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("subir")]
        public async Task<IActionResult> SubirArchivo([FromForm] IFormFile archivo, [FromForm] int usuarioId)
        {
            try
            {
                if (archivo == null || archivo.Length == 0)
                    return BadRequest("Archivo no válido.");

                var uploadsPath = Path.Combine(_env.ContentRootPath, "uploads");
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
                var rutaCompleta = Path.Combine(uploadsPath, nombreArchivo);
                var rutaRelativa = $"/uploads/{nombreArchivo}";

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                var adjunto = new EAdjunto
                {
                    CT_Archivo_ruta = rutaRelativa,
                    CN_Usuario_accion = usuarioId,
                    CF_Fecha_registro = DateTime.Now
                };

                await _repository.AddAsync(adjunto);
                return Ok(adjunto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error al subir archivo: {ex.Message}");
            }
        }
    }
}
