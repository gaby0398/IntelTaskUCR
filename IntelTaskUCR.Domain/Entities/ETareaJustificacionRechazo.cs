using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntelTaskUCR.Domain.Entities
{
    public class ETareaJustificacionRechazo
    {
        
        public int CN_Id_tarea { get; set; }
        public DateTime CF_Fecha_hora_rechazo { get; set; }
        public string CT_Descripcion_rechazo { get; set; } = string.Empty;

        [Key]
        public int CN_Id_tarea_rechazo { get; set; }




       
    }
}
