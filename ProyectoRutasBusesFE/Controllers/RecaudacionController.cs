using Microsoft.AspNetCore.Mvc;
using ProyectoRutasBusesFE.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System;

namespace ProyectoRutasBusesFE.Controllers
{
    public class RecaudacionController : Controller
    {
        
        #region Acciones de apertura de vistas

        public async Task<IActionResult> Index()
        {
            GestorConexionApis objconexion = new GestorConexionApis();
            List<RecaudacionModel> resultado = await objconexion.ListarRecaudaciones();
            List<RutaModel> listaRutas = await objconexion.ListarRutas();
            List<UnidadModel> listaUnidades = await objconexion.ListarUnidades();

            var rutas = listaRutas
                .ToDictionary(r => r.rutaID, r => r.nombreRuta);

            var unidades = listaUnidades
                .ToDictionary(u => u.unidadID, u => u.numeroPlaca);

            ViewBag.Rutas = rutas;
            ViewBag.Unidades = unidades;

            return View(resultado);
        }

        public async Task<IActionResult> AbrirCrearRecaudacion()
        {
            GestorConexionApis objgestor = new GestorConexionApis();

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

            // Pasar precios de las rutas en un diccionario
            ViewBag.PreciosRutas = rutas.ToDictionary(r => r.rutaID.ToString(), r => r.precioPasaje);

            return View();
        }



        [HttpGet]
        public async Task<IActionResult> FiltrarListaRecaudaciones(string RecaudacionBuscar)
        {
            GestorConexionApis objgestor = new GestorConexionApis();

            // Listar todas las recaudaciones
            List<RecaudacionModel> listaRecaudaciones = await objgestor.ListarRecaudaciones();

            // Listar todas las rutas para poder buscar por nombre de ruta
            List<RutaModel> listaRutas = await objgestor.ListarRutas();

            // Si se ha ingresado un término de búsqueda
            if (!string.IsNullOrEmpty(RecaudacionBuscar))
            {
                // Filtrar las rutas que coincidan con el término de búsqueda
                var rutasFiltradas = listaRutas
                    .Where(r => r.nombreRuta.Contains(RecaudacionBuscar, StringComparison.OrdinalIgnoreCase))
                    .Select(r => r.rutaID)
                    .ToList();

                // Filtrar las recaudaciones cuyos rutaID estén en las rutas filtradas
                listaRecaudaciones = listaRecaudaciones
                    .Where(r => rutasFiltradas.Contains(r.rutaID))
                    .ToList();
            }

            // Cargar nuevamente las listas de rutas y unidades para la vista
            List<UnidadModel> listaUnidades = await objgestor.ListarUnidades();
            var rutas = listaRutas.ToDictionary(r => r.rutaID, r => r.nombreRuta);
            var unidades = listaUnidades.ToDictionary(u => u.unidadID, u => u.numeroPlaca);

            ViewBag.Rutas = rutas;
            ViewBag.Unidades = unidades;

            return View("Index", listaRecaudaciones);
        }



        #endregion

        #region Acciones de Mantenimiento sobre RecaudacionModel

        [HttpPost]
        public async Task<IActionResult> GuardarRecaudacion(RecaudacionModel recaudacion)
        {
            // Validar si ya existe una recaudación para la misma ruta y unidad en el día actual
            GestorConexionApis objgestor = new GestorConexionApis();
            var existeRecaudacion = await objgestor.ValidarRecaudacionExistente(recaudacion.rutaID, recaudacion.unidadID, DateTime.Today);


            var resultado = await objgestor.AgregarRecaudacion(recaudacion);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Recaudación agregada exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Ya existe un reporte de lo recaudado para la Ruta {recaudacion.rutaID} y Unidad {recaudacion.unidadID} en la fecha {DateTime.Today.ToShortDateString()}";
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}
