using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Domain.Interfaces;
using IntelTaskUCR.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace IntelTaskUCR.Infrastructure.Repositories
{
    public class TareaRepository : ITareaRepository
    {
        private readonly IntelTaskDbContext _context;
        private readonly INotificacionRepository _notificacionRepository;
        private readonly ITINotificacionUsuarioRepository _tiNotificacionUsuarioRepository;


        public TareaRepository(
         IntelTaskDbContext context,
         INotificacionRepository notificacionRepository,
         ITINotificacionUsuarioRepository tiNotificacionUsuarioRepository)
        {
            _context = context;
            _notificacionRepository = notificacionRepository;
            _tiNotificacionUsuarioRepository = tiNotificacionUsuarioRepository;
        }


        public async Task<IEnumerable<ETarea>> GetAllAsync()
        {
            return await _context.T_Tareas
                .Include(t => t.UsuarioCreador)
                .Include(t => t.UsuarioAsignado)
                .ToListAsync();
        }


        public async Task<ETarea?> GetByIdAsync(int id)
        {
            return await _context.T_Tareas.FindAsync(id);
        }

        public async Task AddAsync(ETarea tarea)
        {
            // 1. Determinar el estado inicial (registrado o asignado)
            byte estadoInicial = (byte)(tarea.CN_Usuario_asignado.HasValue ? 2 : 1);
            tarea.CN_Id_estado = estadoInicial;

            // 2. Guardar la tarea
            await _context.T_Tareas.AddAsync(tarea);
            await _context.SaveChangesAsync();

            // 3. Registrar el cambio en la bitácora
            var bitacora = new EBitacoraCambioEstado
            {
                CN_Id_tarea_permiso = tarea.CN_Id_tarea,
                CN_Id_tipo_documento = 1, // creación
                CN_Id_estado_anterior = estadoInicial,
                CN_Id_estado_nuevo = estadoInicial,
                CF_Fecha_hora_cambio = DateTime.Now,
                CN_Id_usuario_responsable = tarea.CN_Usuario_creador,
                CT_Observaciones = "Creación inicial de la tarea"
            };

            await _context.T_Bitacora_Cambios_Estados.AddAsync(bitacora);
            await _context.SaveChangesAsync();

            // 4. Crear notificación solo si hay usuario asignado
            if (tarea.CN_Usuario_asignado.HasValue)
            {
                // ✅ Obtener el último ID actual de notificación
                int ultimoId = await _context.T_Notificaciones
                    .OrderByDescending(n => n.CN_Id_notificacion)
                    .Select(n => n.CN_Id_notificacion)
                    .FirstOrDefaultAsync();

                int nuevoId = ultimoId + 1;

                // ✅ Crear la notificación con ID manual
                var notificacion = new ENotificacion
                {
                    CN_Id_notificacion = nuevoId,
                    CN_Tipo_notificacion = 1, // Tipo 1 = Tarea asignada
                    CT_Titulo_notificacion = "Nueva tarea asignada",
                    CT_Texto_notificacion = $"Se te ha asignado la tarea: {tarea.CT_Titulo_tarea}",
                    CT_Correo_origen = "notificaciones@inteltask.com",
                    CF_Fecha_registro = DateTime.Now,
                    CF_Fecha_notificacion = DateTime.Now
                };

                await _context.T_Notificaciones.AddAsync(notificacion);
                await _context.SaveChangesAsync();

                // ✅ Buscar correo del usuario asignado
                var usuario = await _context.T_Usuarios
                    .FirstOrDefaultAsync(u => u.CN_Id_usuario == tarea.CN_Usuario_asignado.Value);

                // ✅ Relacionar notificación con el usuario
                var notificacionUsuario = new TINotificacionUsuario
                {
                    CN_Id_notificacion = nuevoId,
                    CN_Id_usuario = tarea.CN_Usuario_asignado.Value,
                    CT_Correo_destino = usuario?.CT_Correo_usuario ?? "correo@desconocido.com"
                };

                await _context.TI_Notificaciones_X_Usuarios.AddAsync(notificacionUsuario);
                await _context.SaveChangesAsync();
            }
        }





        public async Task UpdateAsync(ETarea tarea)
        {
            // 1. Buscar la tarea existente
            var existente = await _context.T_Tareas.FirstOrDefaultAsync(t => t.CN_Id_tarea == tarea.CN_Id_tarea);
            if (existente == null)
                throw new Exception("Tarea no encontrada.");

            // 2. Validar que quien modifica sea el creador o el asignado
            if (existente.CN_Usuario_creador != tarea.CN_Usuario_creador &&
                existente.CN_Usuario_asignado != tarea.CN_Usuario_creador)
                throw new UnauthorizedAccessException("Solo el creador o el usuario asignado puede modificar esta tarea.");

            // 3. Detectar cambio de estado
            bool cambioEstado = existente.CN_Id_estado != tarea.CN_Id_estado;

            // 4. Actualizar los valores
            _context.Entry(existente).CurrentValues.SetValues(tarea);
            await _context.SaveChangesAsync();

            // 5. Si hubo cambio de estado, enviar notificación
            if (cambioEstado && existente.CN_Usuario_asignado.HasValue)
            {
                int ultimoId = await _context.T_Notificaciones
                    .OrderByDescending(n => n.CN_Id_notificacion)
                    .Select(n => n.CN_Id_notificacion)
                    .FirstOrDefaultAsync();

                int nuevoId = ultimoId + 1;

                var notificacion = new ENotificacion
                {
                    CN_Id_notificacion = nuevoId,
                    CN_Tipo_notificacion = 6, // 6 = Cambio de estado de tarea
                    CT_Titulo_notificacion = "Cambio en el estado de una tarea",
                    CT_Texto_notificacion = $"La tarea '{tarea.CT_Titulo_tarea}' cambió de estado.",
                    CT_Correo_origen = "notificaciones@inteltask.com",
                    CF_Fecha_registro = DateTime.Now,
                    CF_Fecha_notificacion = DateTime.Now
                };

                await _context.T_Notificaciones.AddAsync(notificacion);

                var usuario = await _context.T_Usuarios
                    .FirstOrDefaultAsync(u => u.CN_Id_usuario == existente.CN_Usuario_asignado.Value);

                var notificacionUsuario = new TINotificacionUsuario
                {
                    CN_Id_notificacion = nuevoId,
                    CN_Id_usuario = existente.CN_Usuario_asignado.Value,
                    CT_Correo_destino = usuario?.CT_Correo_usuario ?? "correo@desconocido.com"
                };

                await _context.TI_Notificaciones_X_Usuarios.AddAsync(notificacionUsuario);

                await _context.SaveChangesAsync();
            }
        }



        public async Task DeleteAsync(int id)
        {
            var tarea = await _context.T_Tareas.FindAsync(id);
            if (tarea != null)
            {
                _context.T_Tareas.Remove(tarea);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ETarea>> GetTareasPorEstadoAsync(byte estadoId)
        {
            return await _context.T_Tareas
                                 .Where(t => t.CN_Id_estado == estadoId) // Filtra por ambos estados
                                 .ToListAsync();
        }

        public async Task<int> GetCantidadTareasProximasAVencerAsync()
        {
            // Definir un rango de días, por ejemplo, 7 días antes de la fecha límite
            var fechaLimiteProxima = DateTime.Now.AddDays(7);  // Puedes ajustar el valor a lo que consideres como "próximas a vencer"

            return await _context.T_Tareas
                                 .Where(t => t.CF_Fecha_limite <= fechaLimiteProxima && t.CN_Id_estado != 12)  // Filtra tareas que aún no están finalizadas
                                 .CountAsync();  // Devuelve la cantidad
        }


        public async Task<IEnumerable<ETarea>> FiltrarTareasAsync(string? nombre, int? estado, int? prioridad, string? asignado, DateTime? fechaLimite)
        {
            var query = _context.T_Tareas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(t => t.CT_Titulo_tarea.Contains(nombre));

            if (estado.HasValue)
                query = query.Where(t => t.CN_Id_estado == estado.Value);

            if (prioridad.HasValue)
                query = query.Where(t => t.CN_Id_prioridad == prioridad.Value);

            if (!string.IsNullOrWhiteSpace(asignado))
                query = query.Where(t => t.UsuarioAsignado != null && t.UsuarioAsignado.CT_Nombre_usuario.Contains(asignado));

            if (fechaLimite.HasValue)
                query = query.Where(t => t.CF_Fecha_limite.Date == fechaLimite.Value.Date);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ETarea>> GetTareasPorUsuarioAsync(int idUsuario)
        {
            return await _context.T_Tareas
                .Where(t => t.CN_Usuario_creador == idUsuario || t.CN_Usuario_asignado == idUsuario)
                .Include(t => t.UsuarioCreador)
                .Include(t => t.UsuarioAsignado)
                .ToListAsync();
        }




    }
}
