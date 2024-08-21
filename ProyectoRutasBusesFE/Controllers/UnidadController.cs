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
        private readonly RutaController _rutaController;

        public UnidadController(RutaController rutaController)
        {
            _rutaController = rutaController;
        }

        #region Acciones de apertura de vistas

        public async Task<IActionResult> Index()
        {
            GestorConexionApis objconexion = new GestorConexionApis();
            List<UnidadModel> resultado = await objconexion.ListarUnidades();

            // Obtener todas las rutas
            List<RutaModel> rutas = await objconexion.ListarRutas();

            // Crear un diccionario para mapear rutaID a nombre de la ruta
            var rutasDiccionario = rutas.ToDictionary(r => r.rutaID, r => r.nombreRuta);

            // Pasar el diccionario a la vista mediante ViewBag
            ViewBag.Rutas = rutasDiccionario;

            return View(resultado);
        }

        public async Task<IActionResult> Detalles(int id)
        {
            GestorConexionApis objconexion = new GestorConexionApis();

            // Obtener las unidades filtradas por rutaID
            List<UnidadModel> unidades = await objconexion.ListarUnidades();
            var unidadesFiltradas = unidades.Where(u => u.rutaID == id).ToList();

            // Obtener la lista de usuarios y choferes
            List<UsuarioModel> listaUsuarios = await objconexion.ListarUsuarios();
            List<ChoferModel> listaChoferes = await objconexion.ListarChoferes();

            // Crear un diccionario para mapear unidadID al nombre completo del chofer
            var choferesDiccionario = listaChoferes
                .ToDictionary(
                    c => c.unidadID,
                    c => listaUsuarios.FirstOrDefault(u => u.usuarioID == c.usuarioID)?.nombre + " " + listaUsuarios.FirstOrDefault(u => u.usuarioID == c.usuarioID)?.apellido ?? "Sin Asignar"
                );

            ViewBag.Choferes = choferesDiccionario;

            // Obtener todas las rutas
            List<RutaModel> rutas = await objconexion.ListarRutas();
            var rutasDiccionario = rutas.ToDictionary(r => r.rutaID, r => r.nombreRuta);
            ViewBag.Rutas = rutasDiccionario;

            return View(unidadesFiltradas);
        }



        public async Task<IActionResult> Mantenimiento()
        {
            GestorConexionApis objconexion = new GestorConexionApis();

            // Obtener todas las unidades
            List<UnidadModel> unidades = await objconexion.ListarUnidades();

            // Obtener todas las rutas
            List<RutaModel> rutas = await objconexion.ListarRutas();

            // Crear un diccionario para mapear rutaID a nombre de la ruta
            var rutasDiccionario = rutas.ToDictionary(r => r.rutaID, r => r.nombreRuta);

            // Pasar el diccionario a la vista mediante ViewBag (si es necesario)
            ViewBag.Rutas = rutasDiccionario;

            // Pasar las unidades al modelo de la vista
            return View(unidades);
        }


        public async Task<IActionResult> AbrirCrearUnidadAsync()
        {
            // Obtener las rutas disponibles desde el RutaController
            var rutas = await _rutaController.GetRutasSelectList();

            ViewBag.Rutas = rutas;

            ViewBag.TiposMotor = new List<SelectListItem>
            {
                new SelectListItem { Value = "Diesel", Text = "Diesel" },
                new SelectListItem { Value = "Electrico", Text = "Eléctrico" },
                new SelectListItem { Value = "Hibrido", Text = "Híbrido" },
                new SelectListItem { Value = "Gasolina", Text = "Gasolina" }
            };

            ViewBag.EstadosUnidad = new List<SelectListItem>
            {
                new SelectListItem { Value = "Disponible", Text = "Disponible" },
                new SelectListItem { Value = "EnReparacion", Text = "En Reparación" },
                new SelectListItem { Value = "Accidentada", Text = "Accidentada" },
                new SelectListItem { Value = "EnRuta", Text = "En Ruta" },
                new SelectListItem { Value = "Desechada", Text = "Desechada" }
            };
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FiltrarListaUnidades(string UnidadBuscar)
        {
            GestorConexionApis objgestor = new GestorConexionApis();
            List<UnidadModel> listaUnidades = await objgestor.ListarUnidades();

            if (!string.IsNullOrEmpty(UnidadBuscar))
                listaUnidades = listaUnidades.FindAll(item => item.numeroPlaca.Contains(UnidadBuscar)).ToList();

            // Obtener todas las rutas
            List<RutaModel> rutas = await objgestor.ListarRutas();

            // Crear un diccionario para mapear rutaID a nombre de la ruta
            var rutasDiccionario = rutas.ToDictionary(r => r.rutaID, r => r.nombreRuta);

            // Pasar el diccionario a la vista mediante ViewBag
            ViewBag.Rutas = rutasDiccionario;

            return View("Index", listaUnidades);
        }


        [HttpGet]
        public async Task<IActionResult> AbrirEdicionUnidad(int unidadID)
        {
            GestorConexionApis objgestor = new GestorConexionApis();
            List<UnidadModel> lstresultado = await objgestor.ListarUnidades();
            UnidadModel encontrado = lstresultado.FirstOrDefault(u => u.unidadID == unidadID);

            // Obtener la lista de rutas disponibles
            List<RutaModel> rutas = await objgestor.ListarRutas();
            ViewBag.Rutas = rutas.Select(r => new SelectListItem { Value = r.rutaID.ToString(), Text = r.nombreRuta }).ToList();
            
            ViewBag.TiposMotor = new List<SelectListItem>
            {
                new SelectListItem { Value = "Diesel", Text = "Diesel" },
                new SelectListItem { Value = "Electrico", Text = "Eléctrico" },
                new SelectListItem { Value = "Hibrido", Text = "Híbrido" },
                new SelectListItem { Value = "Gasolina", Text = "Gasolina" }
            };

            ViewBag.EstadosUnidad = new List<SelectListItem>
            {
                new SelectListItem { Value = "Disponible", Text = "Disponible" },
                new SelectListItem { Value = "EnReparacion", Text = "En Reparación" },
                new SelectListItem { Value = "Accidentada", Text = "Accidentada" },
                new SelectListItem { Value = "EnRuta", Text = "En Ruta" },
                new SelectListItem { Value = "Desechada", Text = "Desechada" }
            };

            return View(encontrado);
        }


        [HttpGet]
        public async Task<IActionResult> AbrirMantenimientoUnidad(int id)
        {
            GestorConexionApis objgestor = new GestorConexionApis();
            List<UnidadModel> unidades = await objgestor.ListarUnidades();
            UnidadModel unidad = unidades.FirstOrDefault(u => u.unidadID == id);

            // Obtener la lista de rutas disponibles
            List<RutaModel> rutas = await objgestor.ListarRutas();
            ViewBag.Rutas = rutas.Select(r => new SelectListItem { Value = r.rutaID.ToString(), Text = r.nombreRuta }).ToList();


            ViewBag.EstadosUnidad = new List<SelectListItem>
            {
                new SelectListItem { Value = "Disponible", Text = "Disponible" },
                new SelectListItem { Value = "EnReparacion", Text = "En Reparación" },
                new SelectListItem { Value = "Accidentada", Text = "Accidentada" },
                new SelectListItem { Value = "EnRuta", Text = "En Ruta" },
                new SelectListItem { Value = "Desechada", Text = "Desechada" }
            };

            return View(unidad);
        }


        #endregion

        #region Acciones de Mantenimiento sobre UnidadModel

        [HttpPost]
        public async Task<IActionResult> GuardarUnidad(UnidadModel unidad)
        {
            GestorConexionApis objgestor = new GestorConexionApis();
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
        public async Task<IActionResult> GuardarMantenimiento(UnidadModel unidad)
        {
            if (ModelState.IsValid)
            {
                GestorConexionApis objgestor = new GestorConexionApis();
                var resultado = await objgestor.ModificarUnidad(unidad);
                if (resultado)
                {
                    TempData["SuccessMessage"] = "Estado de la unidad actualizado correctamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al actualizar el estado de la unidad.";
                }
                return RedirectToAction("Mantenimiento");
            }
            return View("AbrirMantenimientoUnidad", unidad);
        }


        [HttpPost]
        public async Task<IActionResult> EditarUnidad(UnidadModel unidad)
        {
            GestorConexionApis objgestor = new GestorConexionApis();
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
            GestorConexionApis objgestor = new GestorConexionApis();
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
