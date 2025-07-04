namespace IntelTaskUCR.Domain.Entities
{
    public class EOficina
    {
        public int CN_Codigo_oficina { get; set; }
        public string CT_Nombre_oficina { get; set; } = string.Empty;
        public int? CN_Oficina_encargada { get; set; } // Referencia a otro usuario o entidad (opcional)
    }
}
