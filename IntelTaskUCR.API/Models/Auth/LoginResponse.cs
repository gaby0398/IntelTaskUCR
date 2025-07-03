namespace IntelTaskUCR.API.Models.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string Correo { get; set; }

    }
}
