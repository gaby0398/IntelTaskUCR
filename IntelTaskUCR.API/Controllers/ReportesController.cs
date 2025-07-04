using IntelTaskUCR.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntelTaskUCR.API.Services; // O el namespace donde esté tu clase PdfGeneratorService


namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IntelTaskDbContext _context;

        public ReportesController(IntelTaskDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetReporte(
            [FromQuery] string tipo,
            [FromQuery] string? estado,
            [FromQuery] DateTime? desde,
            [FromQuery] DateTime? hasta,
            [FromQuery] string? usuario)
        {
            // ----------------- TAREAS -----------------
            if (tipo == "tareas")
            {
                var query = _context.T_Tareas
                    .Include(t => t.UsuarioAsignado)  // 👈 Incluir relación
                    .Include(t => t.Estado)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(estado))
                    query = query.Where(p => p.Estado.CT_Estado.Contains(estado));

                if (!string.IsNullOrEmpty(estado) && byte.TryParse(estado, out byte estadoByte))
                    query = query.Where(t => t.CN_Id_estado == estadoByte);

                if (!string.IsNullOrEmpty(usuario))
                    query = query.Where(t => t.UsuarioAsignado != null && t.UsuarioAsignado.CT_Nombre_usuario.Contains(usuario));

                if (desde.HasValue)
                    query = query.Where(t => t.CF_Fecha_limite >= desde.Value);

                if (hasta.HasValue)
                    query = query.Where(t => t.CF_Fecha_limite <= hasta.Value);

                var tareas = await query.Select(t => new
                {
                    Titulo = t.CT_Titulo_tarea,
                    Usuario = t.UsuarioAsignado != null ? t.UsuarioAsignado.CT_Nombre_usuario : "No asignado",
                    Estado = t.Estado.CT_Estado,
                    Fecha = t.CF_Fecha_limite
                }).ToListAsync();

                return Ok(tareas);
            }


            // ----------------- PERMISOS -----------------
            if (tipo == "permisos")
            {
                var query = _context.T_Permisos
                    .Include(p => p.UsuarioCreador)  // 👈 Incluir relación
                    .Include(p => p.Estado)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(estado))
                    query = query.Where(p => p.Estado.CT_Estado.Contains(estado));

                if (!string.IsNullOrEmpty(estado) && byte.TryParse(estado, out byte estadoByte))
                    query = query.Where(p => p.CN_Id_estado == estadoByte);

                if (!string.IsNullOrEmpty(usuario))
                    query = query.Where(p => p.UsuarioCreador != null && p.UsuarioCreador.CT_Nombre_usuario.Contains(usuario));

                if (desde.HasValue)
                    query = query.Where(p => p.CF_Fecha_hora_inicio_permiso >= desde.Value);

                if (hasta.HasValue)
                    query = query.Where(p => p.CF_Fecha_hora_fin_permiso <= hasta.Value);

                var permisos = await query.Select(p => new
                {
                    Titulo = p.CT_Titulo_permiso,
                    Usuario = p.UsuarioCreador != null ? p.UsuarioCreador.CT_Nombre_usuario : "No disponible",
                    Estado = p.Estado.CT_Estado,
                    Fecha = p.CF_Fecha_hora_inicio_permiso
                }).ToListAsync();

                return Ok(permisos);
            }


            return BadRequest("Tipo de reporte no válido. Usa 'tareas' o 'permisos'.");
        }


    }
}
