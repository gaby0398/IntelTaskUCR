namespace IntelTaskUCR.Domain.Entities
{
    public class EBitacoraAccion
    {
        public int CN_Id_bitacora { get; set; }
        public DateTime CF_Fecha_hora_registro { get; set; }
        public int CN_Id_accion { get; set; }
        public int CN_Id_pantalla { get; set; }
        public int CN_Id_usuario { get; set; }
        public string? CT_informacion_importante { get; set; }
        public int CN_Id_tipo_documento { get; set; }
       
    }
}
