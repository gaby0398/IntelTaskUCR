using IntelTaskUCR.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace IntelTaskUCR.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PanelEstadisticasController : ControllerBase
    {
        private readonly IntelTaskDbContext _context;

        public PanelEstadisticasController(IntelTaskDbContext context)
        {
            _context = context;
        }

        // 🔹 Endpoint 1: Mis Tareas Asignadas
        [HttpGet("MisTareas")]
        public IActionResult GetMisTareas()
        {
            var idUsuario = int.Parse(User.Claims.First(c => c.Type == "IdUsuario").Value);

            var total = _context.T_Tareas.Count(t => t.CN_Usuario_asignado == idUsuario);

            return Ok(total);
        }

        [HttpGet("TareasPorVencer")]
        public IActionResult GetTareasPorVencer()
        {
            var idUsuario = int.Parse(User.Claims.First(c => c.Type == "IdUsuario").Value);
            var hoy = DateTime.Today;
            var en3Dias = hoy.AddDays(3);

            var total = _context.T_Tareas
                .Count(t => t.CN_Usuario_asignado == idUsuario &&
                            t.CF_Fecha_limite >= hoy &&
                            t.CF_Fecha_limite <= en3Dias);

            return Ok(total);
        }

        [HttpGet("PermisosSolicitados")]
        public IActionResult GetPermisosSolicitados()
        {
            var idUsuario = int.Parse(User.Claims.First(c => c.Type == "IdUsuario").Value);
            var total = _context.T_Permisos.Count(p => p.CN_Usuario_creador == idUsuario);
            return Ok(total);
        }


    }
}
