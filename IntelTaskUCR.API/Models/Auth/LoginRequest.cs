namespace IntelTaskUCR.API.Models.Auth
{
    public class LoginRequest
    {
        public string Correo { get; set; } = string.Empty;
        public string Contrasenna { get; set; } = string.Empty;
    }
}
