namespace IntelTaskUCR.Domain.Entities
{
    public class ETipoDocumento
    {
        public int CN_Id_tipo_documento { get; set; }             // PK
        public string CT_Nombre_tipo_documento { get; set; } = string.Empty; // Nombre (único)
    }
}
