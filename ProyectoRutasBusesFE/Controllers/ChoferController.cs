using Microsoft.AspNetCore.Mvc;
using ProyectoRutasBusesFE.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace ProyectoRutasBusesFE.Controllers
{
    public class ChoferController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChoferController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region Acciones de apertura de vistas

        public async Task<IActionResult> Index()
        {
            GestorConexionApis objconexion = new GestorConexionApis(_httpContextAccessor);
            List<ChoferModel> resultado = await objconexion.ListarChoferes();
            return View(resultado);
        }

        public IActionResult AbrirCrearChofer()
        {
            ViewBag.Usuarios = new List<SelectListItem>
            {
                // Aquí deberías cargar los usuarios disponibles
            };
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FiltrarListaChoferes(string ChoferBuscar)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            List<ChoferModel> listachofer = await objgestor.ListarChoferes();

            if (!string.IsNullOrEmpty(ChoferBuscar))
                listachofer = listachofer.FindAll(item => item.numeroEmpleado.Contains(ChoferBuscar)).ToList();

            return View("Index", listachofer);
        }

        [HttpGet]
        public async Task<IActionResult> AbrirEdicionChofer(int choferID)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            List<ChoferModel> lstresultado = await objgestor.ListarChoferes();
            ChoferModel encontrado = lstresultado.FirstOrDefault(u => u.choferID == choferID);

            ViewBag.Usuarios = new List<SelectListItem>
            {
                // Aquí deberías cargar los usuarios disponibles
            };
            return View(encontrado);
        }

        #endregion

        #region Acciones de Mantenimiento sobre ChoferModel

        [HttpPost]
        public async Task<IActionResult> GuardarChofer(ChoferModel chofer)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            var resultado = await objgestor.AgregarChofer(chofer);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Chofer agregado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al agregar el chofer.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditarChofer(ChoferModel chofer)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            var resultado = await objgestor.ModificarChofer(chofer);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Chofer modificado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al modificar el chofer.";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> BorrarChofer(int choferID)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            var resultado = await objgestor.EliminarChofer(choferID);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Chofer eliminado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el chofer.";
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}
