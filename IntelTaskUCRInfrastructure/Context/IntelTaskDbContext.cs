using IntelTaskUCR.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IntelTaskUCR.Infrastructure.Context
{
    public class IntelTaskDbContext : DbContext
    {
        // Constructor de la clase que recibe las opciones de configuración para el DbContext
        public IntelTaskDbContext(DbContextOptions<IntelTaskDbContext> options) : base(options)
        {
        }

        // DbSet para la tabla T_Demo
        public DbSet<EDemo> T_Demo { get; set; }

        // DbSet para la tabla T_Usuarios
        public DbSet<EUsuario> T_Usuarios { get; set; }

        // DbSet para la tabla T_Roles 
        public DbSet<ERoles> T_Roles { get; set; }

        public DbSet<EAccion> T_Acciones { get; set; }

        // DbSet para la tabla T_Estados
        public DbSet<EEstado> T_Estados { get; set; } // Agregamos el DbSet para EEstado

        public DbSet<ENotificacion> T_Notificaciones { get; set; }
        public DbSet<EOficina> T_Oficinas { get; set; }
        public DbSet<EPantalla> T_Pantallas { get; set; }

        public DbSet<EPermiso> T_Permisos { get; set; }
        public DbSet<ETarea> T_Tareas { get; set; }

        public DbSet<EPrioridad> T_Prioridades { get; set; }
        public DbSet<ETareaIncumplimiento> T_Tareas_Incumplimientos { get; set; }
        public DbSet<ETareaJustificacionRechazo> T_Tareas_Justificacion_Rechazo { get; set; }

        public DbSet<ETareaSeguimiento> T_Tareas_Seguimiento { get; set; }
        public DbSet<ETipoDocumento> T_Tipos_documentos { get; set; }

        public DbSet<TINotificacionUsuario> TI_Notificaciones_X_Usuarios { get; set; }
        public DbSet<TIRolPantallaAccion> TI_Rol_X_Pantalla_X_Accion { get; set; }

        public DbSet<TIUsuarioOficina> TI_Usuario_X_Oficina { get; set; }
        public DbSet<EFrecuenciaRecordatorio> T_Frecuencia_Recordatorio { get; set; }

        public DbSet<EDiaNoHabil> T_Dias_No_Habiles { get; set; }
        public DbSet<EComplejidad> T_Complejidades { get; set; }

        public DbSet<EAdjunto> T_Adjuntos { get; set; }

        public DbSet<EBitacoraAccion> T_Bitacora_Acciones { get; set; }

        public DbSet<EBitacoraCambioEstado> T_Bitacora_Cambios_Estados { get; set; }

        public DbSet<EAdjuntoXTarea> T_Adjuntos_X_Tareas { get; set; }

    

        // Configuración del modelo
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la tabla T_Demo
            modelBuilder.Entity<EDemo>().ToTable("T_Demo");
            modelBuilder.Entity<EDemo>().HasKey(d => d.TN_Codigo); // Definir la clave primaria

            // Configuración de la tabla T_Usuarios
            modelBuilder.Entity<EUsuario>().ToTable("T_Usuarios"); // Especificamos el nombre de la tabla
            modelBuilder.Entity<EUsuario>().HasKey(u => u.CN_Id_usuario); // Definir la clave primaria
            modelBuilder.Entity<EUsuario>().Property(u => u.CT_Nombre_usuario).HasMaxLength(100); // Configuramos una restricción de longitud
            modelBuilder.Entity<EUsuario>().Property(u => u.CT_Correo_usuario).HasMaxLength(150); // Configuramos una restricción de longitud
            modelBuilder.Entity<EUsuario>().Property(u => u.CB_Estado_usuario).IsRequired(); // Definir como obligatorio el campo estado

            // Configuración de la tabla T_Roles
            modelBuilder.Entity<ERoles>().ToTable("T_Roles"); // Especificamos el nombre de la tabla
            modelBuilder.Entity<ERoles>().HasKey(r => r.CN_Id_rol); // Definir la clave primaria
            modelBuilder.Entity<ERoles>().Property(r => r.CT_Nombre_rol).HasMaxLength(100); // Configuramos una restricción de longitud
            modelBuilder.Entity<ERoles>().Property(r => r.CN_Jerarquia).IsRequired(); // Definir como obligatorio el campo de jerarquía

            // Configuración de la tabla T_Acciones
            modelBuilder.Entity<EAccion>().ToTable("T_Acciones"); // Especificamos el nombre de la tabla
            modelBuilder.Entity<EAccion>().HasKey(a => a.CN_Id_accion); // Definir la clave primaria
            modelBuilder.Entity<EAccion>().Property(a => a.CT_Descripcion_accion).HasMaxLength(255); // Configuramos una restricción de longitud

            // Configuración de la tabla T_Estados
            modelBuilder.Entity<EEstado>().ToTable("T_Estados"); // Especificamos el nombre de la tabla
            modelBuilder.Entity<EEstado>().HasKey(e => e.CN_Id_estado); // Definir la clave primaria
            modelBuilder.Entity<EEstado>().Property(e => e.CT_Estado).HasMaxLength(100); // Configuramos una restricción de longitud
            modelBuilder.Entity<EEstado>().Property(e => e.CT_Descripcion).HasMaxLength(500); // Configuramos una restricción de longitud

            // Configuración para la tabla T_Notificaciones
            modelBuilder.Entity<ENotificacion>().ToTable("T_Notificaciones");
            modelBuilder.Entity<ENotificacion>().HasKey(n => n.CN_Id_notificacion);

            // Configuración para la tabla T_Oficinas
            modelBuilder.Entity<EOficina>().ToTable("T_Oficinas");
            modelBuilder.Entity<EOficina>().HasKey(o => o.CN_Codigo_oficina);

            // Configuración para la tabla T_Pantallas
            modelBuilder.Entity<EPantalla>().ToTable("T_Pantallas");
            modelBuilder.Entity<EPantalla>().HasKey(p => p.CN_Id_pantalla);

            // Configuración para la tabla T_Permisos
            modelBuilder.Entity<EPermiso>().ToTable("T_Permisos");
            modelBuilder.Entity<EPermiso>().HasKey(p => p.CN_Id_permiso);

            // Configuración para la tabla T_Prioridades
            modelBuilder.Entity<EPrioridad>().ToTable("T_Prioridades");
            modelBuilder.Entity<EPrioridad>().HasKey(p => p.CN_Id_prioridad);

            modelBuilder.Entity<ETarea>().ToTable("T_Tareas");
            modelBuilder.Entity<ETarea>().HasKey(t => t.CN_Id_tarea);

            modelBuilder.Entity<ETareaIncumplimiento>().ToTable("T_Tareas_Incumplimientos");
            modelBuilder.Entity<ETareaIncumplimiento>().HasKey(t => t.CN_Id_tarea_incumplimiento);
            
            modelBuilder.Entity<ETareaJustificacionRechazo>().ToTable("T_Tareas_Justificacion_Rechazo");
            modelBuilder.Entity<ETareaJustificacionRechazo>().HasKey(j => j.CN_Id_tarea_rechazo);

            modelBuilder.Entity<ETareaSeguimiento>().ToTable("T_Tareas_Seguimiento");
            modelBuilder.Entity<ETareaSeguimiento>().HasKey(s => s.CN_Id_seguimiento);

            modelBuilder.Entity<ETipoDocumento>().ToTable("T_Tipos_documentos");
            modelBuilder.Entity<ETipoDocumento>().HasKey(t => t.CN_Id_tipo_documento);
            modelBuilder.Entity<ETipoDocumento>().HasIndex(t => t.CT_Nombre_tipo_documento).IsUnique();

            modelBuilder.Entity<TINotificacionUsuario>().ToTable("TI_Notificaciones_X_Usuarios");
            modelBuilder.Entity<TINotificacionUsuario>().HasKey(n => new { n.CN_Id_notificacion, n.CN_Id_usuario });

            modelBuilder.Entity<TIRolPantallaAccion>().ToTable("TI_Rol_X_Pantalla_X_Accion");
            modelBuilder.Entity<TIRolPantallaAccion>().HasKey(x => new { x.CN_Id_pantalla, x.CN_Id_accion, x.CN_Id_rol });

            modelBuilder.Entity<TIUsuarioOficina>().ToTable("TI_Usuario_X_Oficina");
            modelBuilder.Entity<TIUsuarioOficina>().HasKey(x => new { x.CN_Id_usuario, x.CN_Codigo_oficina });

            modelBuilder.Entity<EFrecuenciaRecordatorio>().ToTable("T_Frecuencia_Recordatorio");
            modelBuilder.Entity<EFrecuenciaRecordatorio>().HasKey(x => x.CN_Id_recordatorio);

            modelBuilder.Entity<EDiaNoHabil>().ToTable("T_Dias_No_Habiles");
            modelBuilder.Entity<EDiaNoHabil>().HasKey(x => x.CN_Id_dias_no_habiles);

            modelBuilder.Entity<EComplejidad>().ToTable("T_Complejidades");
            modelBuilder.Entity<EComplejidad>().HasKey(x => x.CN_Id_complejidad);

            modelBuilder.Entity<EAdjunto>().ToTable("T_Adjuntos");
            modelBuilder.Entity<EAdjunto>().HasKey(x => x.CN_Id_adjuntos);

            modelBuilder.Entity<EBitacoraAccion>().ToTable("T_Bitacora_Acciones");
            modelBuilder.Entity<EBitacoraAccion>().HasKey(x => x.CN_Id_bitacora);

            modelBuilder.Entity<EBitacoraCambioEstado>().ToTable("T_Bitacora_Cambios_Estados");
            modelBuilder.Entity<EBitacoraCambioEstado>().HasKey(x => x.CN_Id_cambio_estado);

            modelBuilder.Entity<EAdjuntoXTarea>().ToTable("T_Adjuntos_X_Tareas");
            modelBuilder.Entity<EAdjuntoXTarea>().HasKey(x => new { x.CN_Id_tarea, x.CN_Id_adjuntos });

            // Tarea: Usuario asignado y creador
            modelBuilder.Entity<ETarea>()
                .HasOne(t => t.UsuarioAsignado)
                .WithMany()
                .HasForeignKey(t => t.CN_Usuario_asignado)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ETarea>()
                .HasOne(t => t.UsuarioCreador)
                .WithMany()
                .HasForeignKey(t => t.CN_Usuario_creador)
                .OnDelete(DeleteBehavior.Restrict);

            // Permiso: Usuario creador
            modelBuilder.Entity<EPermiso>()
                .HasOne(p => p.UsuarioCreador)
                .WithMany()
                .HasForeignKey(p => p.CN_Usuario_creador)
                .OnDelete(DeleteBehavior.Restrict);

            

            modelBuilder.Entity<ETarea>()
                .HasOne(t => t.Estado)
                .WithMany()
                .HasForeignKey(t => t.CN_Id_estado);

            modelBuilder.Entity<EPermiso>()
                .HasOne(p => p.Estado)
                .WithMany()
                .HasForeignKey(p => p.CN_Id_estado);



        }
    }
}
