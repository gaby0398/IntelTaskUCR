namespace IntelTaskUCR.Domain.Entities
{
    public class EBitacoraCambioEstado
    {
        public int CN_Id_cambio_estado { get; set; }
        public int CN_Id_tarea_permiso { get; set; }

        public int CN_Id_tipo_documento { get; set; }
        public byte CN_Id_estado_anterior { get; set; }
        public byte CN_Id_estado_nuevo { get; set; }
        public DateTime CF_Fecha_hora_cambio { get; set; }
        public int CN_Id_usuario_responsable { get; set; }
        public string? CT_Observaciones { get; set; }
    }
}
