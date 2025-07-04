using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelTaskUCR.Domain.Entities
{
    public class EEstado
    {
        // Representa el ID del estado (clave primaria)
        public byte CN_Id_estado { get; set; }

        // Nombre del estado
        public string CT_Estado { get; set; } = string.Empty;

        // Descripción del estado
        public string? CT_Descripcion { get; set; }

    }
}
