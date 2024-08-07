using Microsoft.AspNetCore.Mvc;
using ProyectoRutasBusesFE.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace ProyectoRutasBusesFE.Controllers
{
    public class RecaudacionController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RecaudacionController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region Acciones de apertura de vistas

        public async Task<IActionResult> Index()
        {
            GestorConexionApis objconexion = new GestorConexionApis(_httpContextAccessor);
            List<RecaudacionModel> resultado = await objconexion.ListarRecaudaciones();
            return View(resultado);
        }

        public async Task<IActionResult> AbrirCrearRecaudacion()
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);

            // Cargar las rutas disponibles
            List<RutaModel> rutas = await objgestor.ListarRutas();
            ViewBag.Rutas = rutas.Select(r => new SelectListItem
            {
                Value = r.rutaID.ToString(),
                Text = r.nombreRuta
            }).ToList();

            // Cargar las unidades disponibles
            List<UnidadModel> unidades = await objgestor.ListarUnidades();
            ViewBag.Unidades = unidades.Select(u => new SelectListItem
            {
                Value = u.unidadID.ToString(),
                Text = u.numeroPlaca
            }).ToList();

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> FiltrarListaRecaudaciones(string RecaudacionBuscar)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            List<RecaudacionModel> listaRecaudaciones = await objgestor.ListarRecaudaciones();

            if (!string.IsNullOrEmpty(RecaudacionBuscar))
                listaRecaudaciones = listaRecaudaciones.FindAll(item => item.rutaID.ToString().Contains(RecaudacionBuscar)).ToList();

            return View("Index", listaRecaudaciones);
        }

        //[HttpGet]
        //public async Task<IActionResult> AbrirEdicionRecaudacion(int recaudacionID)
        //{
        //    GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
        //    List<RecaudacionModel> lstresultado = await objgestor.ListarRecaudaciones();
        //    RecaudacionModel encontrado = lstresultado.FirstOrDefault(u => u.recaudacionID == recaudacionID);

        //    ViewBag.Rutas = new List<SelectListItem>
        //    {
        //        // Aquí deberías cargar las rutas disponibles
        //    };
        //    ViewBag.Unidades = new List<SelectListItem>
        //    {
        //        // Aquí deberías cargar las unidades disponibles
        //    };
        //    return View(encontrado);
        //}

        #endregion

        #region Acciones de Mantenimiento sobre RecaudacionModel

        [HttpPost]
        public async Task<IActionResult> GuardarRecaudacion(RecaudacionModel recaudacion)
        {
            GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
            var resultado = await objgestor.AgregarRecaudacion(recaudacion);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Recaudación agregada exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al agregar la recaudación.";
            }
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public async Task<IActionResult> EditarRecaudacion(RecaudacionModel recaudacion)
        //{
        //    GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
        //    var resultado = await objgestor.ModificarRecaudacion(recaudacion);
        //    if (resultado)
        //    {
        //        TempData["SuccessMessage"] = "Recaudación modificada exitosamente.";
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = "Error al modificar la recaudación.";
        //    }
        //    return RedirectToAction("Index");
        //}

        //[HttpGet]
        //public async Task<IActionResult> BorrarRecaudacion(int recaudacionID)
        //{
        //    GestorConexionApis objgestor = new GestorConexionApis(_httpContextAccessor);
        //    var resultado = await objgestor.EliminarRecaudacion(recaudacionID);
        //    if (resultado)
        //    {
        //        TempData["SuccessMessage"] = "Recaudación eliminada exitosamente.";
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = "Error al eliminar la recaudación.";
        //    }
        //    return RedirectToAction("Index");
        //}

        #endregion
    }
}
