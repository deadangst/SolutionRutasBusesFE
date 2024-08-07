using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProyectoRutasBusesFE.Models;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ProyectoRutasBusesFE.Controllers
{
    public class AutenticacionController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly GestorConexionApis _gestorConexionApis;

        public AutenticacionController(IConfiguration config, IHttpContextAccessor httpContextAccessor, GestorConexionApis gestorConexionApis)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _gestorConexionApis = gestorConexionApis;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] UserLoginModel loginModel)
        {
            if (loginModel == null)
                return BadRequest("Invalid client request");

            var usuario = await _gestorConexionApis.ObtenerUsuarioPorEmail(loginModel.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, usuario.passwordHash))
                return Unauthorized();

            var token = GenerateJwtToken(loginModel.Email);

            // Guardar el token en la sesión
            HttpContext.Session.SetString("JWToken", token);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AbrirCrearUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GuardarUsuario(UsuarioModel usuario)
        {
            var resultado = await _gestorConexionApis.AgregarUsuario(usuario);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Usuario agregado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al agregar el usuario.";
            }
            return RedirectToAction("Index");
        }

        private string GenerateJwtToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
