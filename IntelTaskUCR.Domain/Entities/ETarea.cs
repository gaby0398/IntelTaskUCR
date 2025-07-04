namespace IntelTaskUCR.Domain.Entities
{
    public class ETarea
    {
        public int CN_Id_tarea { get; set; }
        public int? CN_Tarea_origen { get; set; }
        public string? CT_Titulo_tarea { get; set; }
        public string CT_Descripcion_tarea { get; set; } = string.Empty;
        public string? CT_Descripcion_espera { get; set; }
        public byte CN_Id_complejidad { get; set; }
        public byte? CN_Id_estado { get; set; }
        public byte CN_Id_prioridad { get; set; }
        public string? CN_Numero_GIS { get; set; }
        public DateTime CF_Fecha_asignacion { get; set; }
        public DateTime CF_Fecha_limite { get; set; }
        public DateTime CF_Fecha_finalizacion { get; set; }
        public int CN_Usuario_creador { get; set; }
        public int? CN_Usuario_asignado { get; set; }

        public EUsuario? UsuarioAsignado { get; set; }
        public EUsuario UsuarioCreador { get; set; }

        public EEstado? Estado { get; set; }


    }
}
