using Microsoft.AspNetCore.Mvc;
using ProyectoRutasBusesFE.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace ProyectoRutasBusesFE.Controllers
{
    public class UnidadController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnidadController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region Acciones de apertura de vistas

        public async Task<IActionResult> Index()
        {
            GestorConexionApis objconexion = new GestorConexionApis(_httpContextAccessor);
            List<UnidadModel> resultado = await objconexion.ListarUnidades();
            return View(resultado);
        }

        public IActionResult AbrirCrearUnidad()
        {
            ViewBag.Rutas = new List<SelectListItem>
            {
                // Aquí deberías cargar las rutas disponibles
            };
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FiltrarListaUnidades(string UnidadBuscar)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            List<UnidadModel> listaUnidades = await objgestor.ListarUnidades();

            if (!string.IsNullOrEmpty(UnidadBuscar))
                listaUnidades = listaUnidades.FindAll(item => item.numeroPlaca.Contains(UnidadBuscar)).ToList();

            return View("Index", listaUnidades);
        }

        [HttpGet]
        public async Task<IActionResult> AbrirEdicionUnidad(int unidadID)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            List<UnidadModel> lstresultado = await objgestor.ListarUnidades();
            UnidadModel encontrado = lstresultado.FirstOrDefault(u => u.unidadID == unidadID);

            ViewBag.Rutas = new List<SelectListItem>
            {
                // Aquí deberías cargar las rutas disponibles
            };
            return View(encontrado);
        }

        #endregion

        #region Acciones de Mantenimiento sobre UnidadModel

        [HttpPost]
        public async Task<IActionResult> GuardarUnidad(UnidadModel unidad)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            var resultado = await objgestor.AgregarUnidad(unidad);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Unidad agregada exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al agregar la unidad.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditarUnidad(UnidadModel unidad)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            var resultado = await objgestor.ModificarUnidad(unidad);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Unidad modificada exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al modificar la unidad.";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> BorrarUnidad(int unidadID)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            var resultado = await objgestor.EliminarUnidad(unidadID);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Unidad eliminada exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar la unidad.";
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}
