using Microsoft.AspNetCore.Mvc;
using ProyectoRutasBusesFE.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace ProyectoRutasBusesFE.Controllers
{
    public class RutaController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RutaController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region Acciones de apertura de vistas

        public async Task<IActionResult> Index()
        {
            GestorConexionApis objconexion = new GestorConexionApis(_httpContextAccessor);
            List<RutaModel> resultado = await objconexion.ListarRutas();
            return View(resultado);
        }

        public IActionResult AbrirCrearRuta()
        {
            ViewBag.Supervisores = new List<SelectListItem>
            {
                // Aquí deberías cargar los supervisores disponibles
            };
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FiltrarListaRutas(string RutaBuscar)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            List<RutaModel> listarutas = await objgestor.ListarRutas();

            if (!string.IsNullOrEmpty(RutaBuscar))
                listarutas = listarutas.FindAll(item => item.nombreRuta.Contains(RutaBuscar)).ToList();

            return View("Index", listarutas);
        }

        [HttpGet]
        public async Task<IActionResult> AbrirEdicionRuta(int rutaID)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            List<RutaModel> lstresultado = await objgestor.ListarRutas();
            RutaModel encontrado = lstresultado.FirstOrDefault(u => u.rutaID == rutaID);

            ViewBag.Supervisores = new List<SelectListItem>
            {
                // Aquí deberías cargar los supervisores disponibles
            };
            return View(encontrado);
        }

        #endregion

        #region Acciones de Mantenimiento sobre RutaModel

        [HttpPost]
        public async Task<IActionResult> GuardarRuta(RutaModel ruta)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            var resultado = await objgestor.AgregarRuta(ruta);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Ruta agregada exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al agregar la ruta.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditarRuta(RutaModel ruta)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            var resultado = await objgestor.ModificarRuta(ruta);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Ruta modificada exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al modificar la ruta.";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> BorrarRuta(int rutaID)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            var resultado = await objgestor.EliminarRuta(rutaID);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Ruta eliminada exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar la ruta.";
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}
