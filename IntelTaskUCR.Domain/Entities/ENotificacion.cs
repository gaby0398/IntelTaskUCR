namespace IntelTaskUCR.Domain.Entities
{
    public class ENotificacion
    {
        public int CN_Id_notificacion { get; set; }
        public int CN_Tipo_notificacion { get; set; }
        public string CT_Titulo_notificacion { get; set; } = string.Empty;
        public string CT_Texto_notificacion { get; set; } = string.Empty;
        public string CT_Correo_origen { get; set; } = string.Empty;
        public DateTime CF_Fecha_registro { get; set; }
        public DateTime CF_Fecha_notificacion { get; set; }
        public int? CN_Id_recordatorio { get; set; }

       
      


    }
}
