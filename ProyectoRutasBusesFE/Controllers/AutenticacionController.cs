using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoRutasBusesFE.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ProyectoRutasBusesFE.Extensions;

namespace ProyectoRutasBusesFE.Controllers
{
    public class AutenticacionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(UsuarioModel P_usuario)
        {
            UsuarioModel objusuario = P_usuario;

            if (objusuario != null)
            {
                GestorConexionApis objgestor = new GestorConexionApis();
                List<PerfilModel> perfiles = await objgestor.AutorizacionesPorUsuarios(objusuario);

                // Aquí se asume que cada usuario tiene un solo perfil relacionado, si no es el caso, se debería ajustar
                if (perfiles.Count > 0)
                {
                    objusuario.usuarioID = perfiles[0].codigoPerfil; // Asigna el código de perfil al usuarioID
                }

                HttpContext.Session.SetObject("UsuarioAutenticado", objusuario);

                var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, objusuario.email),
            new Claim(ClaimTypes.GivenName, objusuario.nombre),  // Agregar el nombre
            new Claim(ClaimTypes.Surname, objusuario.apellido),  // Agregar el apellido
            new Claim("Usuario", objusuario.email)
        };

                foreach (PerfilModel item in perfiles)
                    claims.Add(new Claim(ClaimTypes.Role, item.descripcion)); // Validar con descripción del perfil

                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Autenticacion");
        }





        [HttpGet]
        public IActionResult AbrirCrearUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GuardarUsuario(UsuarioModel usuario)
        {
            GestorConexionApis objgestor = new GestorConexionApis();
            var resultado = await objgestor.AgregarUsuario(usuario);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Usuario agregado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al agregar el usuario.";
            }
            return RedirectToAction("Index", "Autenticacion");
        }

        [HttpGet]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Autenticacion");
        }
}
}
