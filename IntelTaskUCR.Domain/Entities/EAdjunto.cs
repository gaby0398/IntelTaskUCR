namespace IntelTaskUCR.Domain.Entities
{
    public class EAdjunto
    {
        public int CN_Id_adjuntos { get; set; }
        //public int CN_Documento { get; set; }
        public string CT_Archivo_ruta { get; set; } = string.Empty;
        public int CN_Usuario_accion { get; set; }
        public DateTime CF_Fecha_registro { get; set; }


    }
}
