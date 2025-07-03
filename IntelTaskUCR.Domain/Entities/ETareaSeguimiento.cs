namespace IntelTaskUCR.Domain.Entities
{
    public class ETareaSeguimiento
    {
        public int CN_Id_seguimiento { get; set; }      // PK
        public int CN_Id_tarea { get; set; }            // FK a la tarea
        public string? CT_Comentario { get; set; }      // Comentario opcional
        public DateTime CF_Fecha_seguimiento { get; set; } // Fecha del seguimiento
    }
}
