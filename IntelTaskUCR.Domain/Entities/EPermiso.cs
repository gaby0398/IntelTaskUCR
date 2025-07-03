namespace IntelTaskUCR.Domain.Entities
{
    public class EPermiso
    {
        public int CN_Id_permiso { get; set; }
        public string CT_Titulo_permiso { get; set; } = string.Empty;
        public string CT_Descripcion_permiso { get; set; } = string.Empty;
        public byte CN_Id_estado { get; set; } // Referencia a un estado
        public string? CT_Descripcion_rechazo { get; set; } = string.Empty;
        public DateTime CF_Fecha_hora_registro { get; set; }
        public DateTime CF_Fecha_hora_inicio_permiso { get; set; }
        public DateTime CF_Fecha_hora_fin_permiso { get; set; }
        public int CN_Usuario_creador { get; set; } // Referencia a usuario creador

        public EUsuario? UsuarioCreador { get; set; }


        public EEstado? Estado { get; set; }

    }
}
