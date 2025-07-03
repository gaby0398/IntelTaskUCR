using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IntelTaskUCR.API.Models.Auth;
using IntelTaskUCR.Domain.Entities;
using IntelTaskUCR.Infrastructure.Context;

namespace IntelTaskUCR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IntelTaskDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(IntelTaskDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Buscar usuario por correo
            var usuario = _context.T_Usuarios.FirstOrDefault(u =>
                u.CT_Correo_usuario == request.Correo && u.CB_Estado_usuario == true);

            if (usuario == null)
                return Unauthorized("Credenciales incorrectas.");

            // Verificar la contraseña utilizando BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Contrasenna, usuario.CT_Contrasenna);
            if (!isPasswordValid)
            {
                return Unauthorized("Credenciales incorrectas.");
            }

            // Crear claims para el JWT
            var claims = new[]
            {
                new Claim("IdUsuario", usuario.CN_Id_usuario.ToString()),
                new Claim("NombreUsuario", usuario.CT_Nombre_usuario),
                new Claim("IdRol", usuario.CN_Id_rol.ToString())
            };

            // Configuración del JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"]!)),
                signingCredentials: creds
            );

            // Retornar el JWT junto con los datos del usuario
            return Ok(new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                NombreUsuario = usuario.CT_Nombre_usuario,
                IdUsuario = usuario.CN_Id_usuario,
                IdRol = usuario.CN_Id_rol,
                Correo = usuario.CT_Correo_usuario
            });
        }
    }
}
