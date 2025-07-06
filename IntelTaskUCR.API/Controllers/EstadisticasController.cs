using IntelTaskUCR.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadisticasController : ControllerBase
    {
        private readonly IntelTaskDbContext _context;

        public EstadisticasController(IntelTaskDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEstadisticas()
        {
            var usuariosActivos = await _context.T_Usuarios
                .Where(u => u.CB_Estado_usuario
                    == true || u.CB_Estado_usuario == true) // uno de los dos debe existir
                .Select(u => new { nombre = u.CT_Nombre_usuario, correo = u.CT_Correo_usuario })
                .ToListAsync();

            var tareasEnProceso = await _context.T_Tareas
                .Where(t => t.CN_Id_estado == 2) // 2 = En proceso (ajustalo si es otro ID)
                .Select(t => new { titulo = t.CT_Titulo_tarea, fecha = t.CF_Fecha_limite })
                .ToListAsync();

            var permisosSolicitados = await _context.T_Permisos
                .Select(p => new { motivo = p.CT_Descripcion_permiso, fecha = p.CF_Fecha_hora_inicio_permiso })
                .ToListAsync();

            var tareasProximas = await _context.T_Tareas
    .Where(t => t.CF_Fecha_limite.Date <= DateTime.Today.AddDays(3))
    .Select(t => new { titulo = t.CT_Titulo_tarea, fechaLimite = t.CF_Fecha_limite })
    .ToListAsync();


            return Ok(new
            {
                usuariosActivos = new
                {
                    cantidad = usuariosActivos.Count,
                    lista = usuariosActivos
                },
                tareasEnProceso = new
                {
                    cantidad = tareasEnProceso.Count,
                    lista = tareasEnProceso
                },
                permisosRegistrados = new
                {
                    cantidad = permisosSolicitados.Count,
                    lista = permisosSolicitados
                },
                tareasProximasAVencer = new
                {
                    cantidad = tareasProximas.Count,
                    lista = tareasProximas
                }
            });
        }
    }
}
