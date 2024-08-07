using Microsoft.AspNetCore.Mvc;
using ProyectoRutasBusesFE.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace ProyectoRutasBusesFE.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region Acciones de apertura de vistas

        public async Task<IActionResult> Index()
        {
            GestorConexionApis objconexion = new GestorConexionApis(_httpContextAccessor);
            List<UsuarioModel> resultado = await objconexion.ListarUsuarios();
            return View(resultado);
        }

        public IActionResult AbrirCrearUsuario()
        {
            ViewBag.TipoUsuarios = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "SuperAdmin" },
                new SelectListItem { Value = "2", Text = "Supervisor" },
                new SelectListItem { Value = "3", Text = "Mecanico" },
                new SelectListItem { Value = "4", Text = "Chofer" }
            };
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FiltrarListaUsuarios(string UsuarioBuscar)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            List<UsuarioModel> listausuario = await objgestor.ListarUsuarios();

            if (!string.IsNullOrEmpty(UsuarioBuscar))
                listausuario = listausuario.FindAll(item => item.nombre.Contains(UsuarioBuscar)).ToList();

            return View("Index", listausuario);
        }

        [HttpGet]
        public async Task<IActionResult> AbrirEdicionUsuario(int usuarioID)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            List<UsuarioModel> lstresultado = await objgestor.ListarUsuarios();
            UsuarioModel encontrado = lstresultado.FirstOrDefault(u => u.usuarioID == usuarioID);

            ViewBag.TipoUsuarios = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "SuperAdmin" },
                new SelectListItem { Value = "2", Text = "Supervisor" },
                new SelectListItem { Value = "3", Text = "Mecanico" },
                new SelectListItem { Value = "4", Text = "Chofer" }
            };
            return View(encontrado);
        }

        #endregion

        #region Acciones de Mantenimiento sobre UsuarioModel

        [HttpPost]
        public async Task<IActionResult> GuardarUsuario(UsuarioModel usuario)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            var resultado = await objgestor.AgregarUsuario(usuario);
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

        [HttpPost]
        public async Task<IActionResult> EditarUsuario(UsuarioModel usuario)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            // Obtener el usuario existente
            List<UsuarioModel> lstresultado = await objgestor.ListarUsuarios();
            UsuarioModel usuarioExistente = lstresultado.FirstOrDefault(u => u.usuarioID == usuario.usuarioID);

            if (usuario.passwordHash == "********")
            {
                // Mantener la contraseña existente si no se ha cambiado
                usuario.passwordHash = usuarioExistente.passwordHash;
            }

            var resultado = await objgestor.ModificarUsuario(usuario);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Usuario modificado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al modificar el usuario.";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> BorrarUsuario(int usuarioID)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            var resultado = await objgestor.EliminarUsuario(usuarioID);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Usuario eliminado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el usuario.";
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}
