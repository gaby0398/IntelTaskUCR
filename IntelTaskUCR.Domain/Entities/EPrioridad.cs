namespace IntelTaskUCR.Domain.Entities
{
    public class EPrioridad
    {
        public byte CN_Id_prioridad { get; set; }
        public string CT_Nombre_prioridad { get; set; } = string.Empty;
        public string CT_Descripcion_prioridad { get; set; } = string.Empty;
    }
}
