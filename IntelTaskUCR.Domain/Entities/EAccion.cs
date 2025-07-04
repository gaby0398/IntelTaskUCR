using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelTaskUCR.Domain.Entities
{
    public class EAccion
    {
        public int CN_Id_accion { get; set; } // Debería ser un 'int', no 'bool'
        public string CT_Descripcion_accion { get; set; } = string.Empty;
    }

}
