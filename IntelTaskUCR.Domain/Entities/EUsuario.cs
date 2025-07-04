using System;

namespace IntelTaskUCR.Domain.Entities
{
    public class EUsuario
    {
        public int CN_Id_usuario { get; set; } // ID del usuario
        public string CT_Nombre_usuario { get; set; } = string.Empty; // Nombre del usuario
        public string CT_Correo_usuario { get; set; } = string.Empty; // Correo del usuario
        public DateTime? CF_Fecha_nacimiento { get; set; } // Fecha de nacimiento
        public string CT_Contrasenna { get; set; } = string.Empty; // Contraseña
        public bool CB_Estado_usuario { get; set; } // Estado (activo/inactivo)
        public DateTime? CF_Fecha_creacion_usuario { get; set; } // Fecha de creación
        public DateTime? CF_Fecha_modificacion_usuario { get; set; } // Fecha de modificación
        public int CN_Id_rol { get; set; } // ID de rol (referencia a la tabla de roles)
    }
}
