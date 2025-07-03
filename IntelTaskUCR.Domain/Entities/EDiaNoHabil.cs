namespace IntelTaskUCR.Domain.Entities
{
    public class EDiaNoHabil
    {
        public int CN_Id_dias_no_habiles { get; set; }
        public DateTime CF_Fecha_inicio { get; set; }
        public DateTime CF_Fecha_fin { get; set; }
        public string CT_Descripcion { get; set; } = string.Empty;
        public bool CB_Activo { get; set; }
    }
}
