namespace IntelTaskUCR.Domain.Entities
{
    public class TINotificacionUsuario
    {
        public int CN_Id_notificacion { get; set; }     // PK compuesta
        public int CN_Id_usuario { get; set; }          // PK compuesta
        public string CT_Correo_destino { get; set; } = string.Empty;
    }
}
