namespace IntelTaskUCR.API.Models.Usuarios
{
    public class CambiarPasswordDTO
    {
        public string ContrasennaActual { get; set; } = string.Empty;
        public string NuevaContrasenna { get; set; } = string.Empty;
    }
}
