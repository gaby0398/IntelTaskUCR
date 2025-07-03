using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelTaskUCR.Domain.Entities
{
    public class ERoles
    {
        // Representa el ID del rol en la base de datos
        public int CN_Id_rol { get; set; }

        // Nombre del rol (por ejemplo: Administrador, Usuario)
        public string CT_Nombre_rol { get; set; } = string.Empty;

        // Jerarquía del rol (el nivel o rango del rol)
        public int CN_Jerarquia { get; set; }
    }
}
