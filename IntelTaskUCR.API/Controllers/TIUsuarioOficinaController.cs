using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context; // 💡 Asegurate de incluir esto
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TIUsuarioOficinaController : ControllerBase
    {
        private readonly ITIUsuarioOficinaRepository _repository;
        private readonly IntelTaskDbContext _context; // ✅ Inyectamos el contexto

        public TIUsuarioOficinaController(
            ITIUsuarioOficinaRepository repository,
            IntelTaskDbContext context) // ✅ Lo recibimos por constructor
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{idUsuario}/{idOficina}")]
        public async Task<IActionResult> Get(int idUsuario, int idOficina)
        {
            var item = await _repository.GetByIdsAsync(idUsuario, idOficina);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TIUsuarioOficina item)
        {
            await _repository.AddAsync(item);
            return CreatedAtAction(nameof(Get), new { idUsuario = item.CN_Id_usuario, idOficina = item.CN_Codigo_oficina }, item);
        }

        [HttpDelete("{idUsuario}/{idOficina}")]
        public async Task<IActionResult> Delete(int idUsuario, int idOficina)
        {
            await _repository.DeleteAsync(idUsuario, idOficina);
            return NoContent();
        }

        [HttpGet("PorOficina/{idUsuario}")]
        public async Task<IActionResult> GetUsuariosPorOficina(int idUsuario)
        {
            var oficinaUsuario = await _context.TI_Usuario_X_Oficina
                .Where(x => x.CN_Id_usuario == idUsuario)
                .Select(x => x.CN_Codigo_oficina)
                .FirstOrDefaultAsync();

            if (oficinaUsuario == 0)
                return Ok(new List<object>()); // No tiene oficina asignada

            var nombreOficina = await _context.T_Oficinas
                .Where(o => o.CN_Codigo_oficina == oficinaUsuario)
                .Select(o => o.CT_Nombre_oficina)
                .FirstOrDefaultAsync();

            if (nombreOficina == "Oficina de Jefatura General")
            {
                var todos = await (
                    from u in _context.T_Usuarios
                    join x in _context.TI_Usuario_X_Oficina on u.CN_Id_usuario equals x.CN_Id_usuario
                    join o in _context.T_Oficinas on x.CN_Codigo_oficina equals o.CN_Codigo_oficina
                    where u.CB_Estado_usuario == true
                    select new
                    {
                        id = u.CN_Id_usuario,
                        nombre = u.CT_Nombre_usuario,
                        oficina = o.CT_Nombre_oficina
                    }).ToListAsync();

                return Ok(todos);
            }

            var usuariosMismaOficina = await (
                from u in _context.T_Usuarios
                join x in _context.TI_Usuario_X_Oficina on u.CN_Id_usuario equals x.CN_Id_usuario
                join o in _context.T_Oficinas on x.CN_Codigo_oficina equals o.CN_Codigo_oficina
                where x.CN_Codigo_oficina == oficinaUsuario && u.CB_Estado_usuario == true
                select new
                {
                    id = u.CN_Id_usuario,
                    nombre = u.CT_Nombre_usuario,
                    oficina = o.CT_Nombre_oficina
                }).ToListAsync();

            return Ok(usuariosMismaOficina);
        }

    }
}
