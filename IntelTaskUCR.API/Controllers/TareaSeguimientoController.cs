using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareaSeguimientoController : ControllerBase
    {
        private readonly ITareaSeguimientoRepository _repository;
        private readonly INotificacionRepository _notificacionRepository;
        private readonly IntelTaskDbContext _context;

        public TareaSeguimientoController(
            ITareaSeguimientoRepository repository,
            INotificacionRepository notificacionRepository,
            IntelTaskDbContext context)
        {
            _repository = repository;
            _notificacionRepository = notificacionRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ETareaSeguimiento item)
        {
            // ✅ 1. Obtener ID del usuario actual desde el token JWT
            int idUsuarioActual = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
            Console.WriteLine("👤 ID del usuario actual: " + idUsuarioActual);

            // ✅ 2. Agregar el seguimiento
            await _repository.AddAsync(item);

            // ✅ 3. Buscar la tarea y los usuarios relacionados
            var tarea = await _context.T_Tareas
                .Include(t => t.UsuarioCreador)
                .Include(t => t.UsuarioAsignado)
                .FirstOrDefaultAsync(t => t.CN_Id_tarea == item.CN_Id_tarea);

            if (tarea != null)
            {
                Console.WriteLine("👤 Creador tarea: " + tarea.CN_Usuario_creador);
                Console.WriteLine("👤 Asignado tarea: " + tarea.CN_Usuario_asignado);

                string? correoOrigen = null;
                string? correoDestino = null;

                if (idUsuarioActual == tarea.CN_Usuario_creador && tarea.UsuarioAsignado != null)
                {
                    correoOrigen = tarea.UsuarioCreador?.CT_Correo_usuario;
                    correoDestino = tarea.UsuarioAsignado?.CT_Correo_usuario;
                }
                else if (idUsuarioActual == tarea.CN_Usuario_asignado && tarea.UsuarioCreador != null)
                {
                    correoOrigen = tarea.UsuarioAsignado?.CT_Correo_usuario;
                    correoDestino = tarea.UsuarioCreador?.CT_Correo_usuario;
                }

                Console.WriteLine("📧 Correo origen: " + correoOrigen);
                Console.WriteLine("📧 Correo destino: " + correoDestino);

                if (!string.IsNullOrEmpty(correoDestino) && !string.IsNullOrEmpty(correoOrigen))
                {
                    // ✅ 4. Obtener último ID de notificación existente
                    var ultimoId = await _context.T_Notificaciones
                        .OrderByDescending(n => n.CN_Id_notificacion)
                        .Select(n => n.CN_Id_notificacion)
                        .FirstOrDefaultAsync();

                    var notificacion = new ENotificacion
                    {
                        CN_Id_notificacion = ultimoId + 1,
                        CN_Tipo_notificacion = 1,
                        CT_Titulo_notificacion = "Nuevo seguimiento agregado",
                        CT_Texto_notificacion = $"📨 El usuario {correoOrigen} agregó un seguimiento a la tarea #{item.CN_Id_tarea}.",
                        CT_Correo_origen = correoOrigen,
                        CF_Fecha_notificacion = DateTime.UtcNow,
                        CF_Fecha_registro = DateTime.UtcNow,
                        CN_Id_recordatorio = 7
                    };

                    Console.WriteLine("⚠️ CREANDO NOTIFICACIÓN:");
                    Console.WriteLine($"ID: {notificacion.CN_Id_notificacion}");
                    Console.WriteLine($"Tarea: {item.CN_Id_tarea}");
                    Console.WriteLine($"Texto: {notificacion.CT_Texto_notificacion}");

                    try
                    {
                        await _notificacionRepository.AddAsync(notificacion);
                        Console.WriteLine("✅ Notificación insertada correctamente");

                        // ✅ 5. Crear relación en TI_Notificaciones_X_Usuarios
                        var notiUsuario = new TINotificacionUsuario
                        {
                            CN_Id_notificacion = notificacion.CN_Id_notificacion,
                            CN_Id_usuario = idUsuarioActual == tarea.CN_Usuario_creador
                                ? tarea.CN_Usuario_asignado!.Value
                                : tarea.CN_Usuario_creador,
                            CT_Correo_destino = correoDestino!
                        };

                        await _context.TI_Notificaciones_X_Usuarios.AddAsync(notiUsuario);
                        await _context.SaveChangesAsync();
                        Console.WriteLine("✅ Relación notificación-usuario guardada correctamente");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("❌ ERROR al insertar la notificación o asignarla: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("⚠️ Correos origen o destino estaban vacíos");
                }
            }
            else
            {
                Console.WriteLine("❌ No se encontró la tarea con ID " + item.CN_Id_tarea);
            }

            return CreatedAtAction(nameof(Get), new { id = item.CN_Id_seguimiento }, item);
        }






        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ETareaSeguimiento item)
        {
            if (id != item.CN_Id_seguimiento)
                return BadRequest("El ID no coincide.");

            await _repository.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("PorTarea/{idTarea}")]
        public async Task<IActionResult> GetPorTarea(int idTarea)
        {
            var seguimientos = await _repository.GetPorTareaAsync(idTarea);
            return Ok(seguimientos);
        }
    }
}
