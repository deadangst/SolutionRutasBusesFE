using Microsoft.AspNetCore.Mvc;
using ProyectoRutasBusesFE.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using ProyectoRutasBusesFE.Services;
using Microsoft.AspNetCore.Authorization;
using ProyectoRutasBusesFE.Extensions;

namespace ProyectoRutasBusesFE.Controllers
{
    [Authorize]
    public class RutaController : Controller
    {

        #region Acciones de apertura de vistas

        public async Task<IActionResult> Index()
        {
            GestorConexionApis objconexion = new GestorConexionApis();

            // Obtener todas las rutas
            List<RutaModel> resultado = await objconexion.ListarRutas();

            // Obtener el usuario autenticado
            var usuarioAutenticado = HttpContext.Session.GetObject<UsuarioModel>("UsuarioAutenticado");

            // Filtrar las rutas si el usuario es Chofer
            if (User.IsInRole("Chofer"))
            {
                // Obtener las rutas asignadas al chofer
                List<ChoferModel> choferes = await objconexion.ListarChoferes();

                // Obtener las rutas asignadas a este chofer
                var rutasAsignadas = choferes
                    .Where(c => c.usuarioID == usuarioAutenticado.usuarioID)
                    .Select(c => c.rutaID)
                    .Distinct()
                    .ToList();

                // Filtrar las rutas que están asignadas al chofer
                resultado = resultado.Where(r => rutasAsignadas.Contains(r.rutaID)).ToList();
            }

            // Obtener todos los usuarios para construir el diccionario de supervisores
            List<UsuarioModel> usuarios = await objconexion.ListarUsuarios();

            // Crear un diccionario para mapear supervisorID a nombre del supervisor
            var supervisoresDiccionario = usuarios.ToDictionary(u => u.usuarioID, u => $"{u.nombre} {u.apellido}");

            // Pasar el diccionario a la vista mediante ViewBag
            ViewBag.Supervisores = supervisoresDiccionario;

            // Si el usuario es SuperAdmin o Supervisor, no se aplica filtro
            return View(resultado);
        }




        public async Task<IActionResult> DataTableRuta()
        {
            GestorConexionApis objconexion = new GestorConexionApis();
            List<RutaModel> resultado = await objconexion.ListarRutas();
            List<UsuarioModel> listaUsuarios = await objconexion.ListarUsuarios();

            // Filtrar solo los supervisores
            var supervisores = listaUsuarios
                .Where(u => u.tipoUsuarioID == 2)
                .ToDictionary(u => u.usuarioID, u => $"{u.nombre} {u.apellido}");

            ViewBag.Supervisores = supervisores;

            return View(resultado);
        }


        public async Task<IActionResult> AbrirCrearRuta()
        {
            GestorConexionApis objgestor = new GestorConexionApis();
            List<UsuarioModel> listaUsuarios = await objgestor.ListarUsuarios();

            // Filtrar solo los supervisores
            var supervisores = listaUsuarios
                .Where(u => u.tipoUsuarioID == 2)
                .Select(u => new SelectListItem
                {
                    Value = u.usuarioID.ToString(),
                    Text = $"{u.nombre} {u.apellido}" // Mostrar nombre y apellido en el dropdown
                }).ToList();

            ViewBag.Supervisores = supervisores;

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> FiltrarListaRutas(string RutaBuscar)
        {
            GestorConexionApis objgestor = new GestorConexionApis();
            List<RutaModel> listarutas = await objgestor.ListarRutas();

            if (!string.IsNullOrEmpty(RutaBuscar))
                listarutas = listarutas.FindAll(item => item.nombreRuta.Contains(RutaBuscar)).ToList();

            // Obtener la lista de usuarios para mapear los supervisores
            List<UsuarioModel> listaUsuarios = await objgestor.ListarUsuarios();

            // Filtrar solo los supervisores y crear un diccionario
            var supervisores = listaUsuarios
                .Where(u => u.tipoUsuarioID == 2)
                .ToDictionary(u => u.usuarioID, u => $"{u.nombre} {u.apellido}");

            ViewBag.Supervisores = supervisores;

            return View("DataTableRuta", listarutas);
        }


        [HttpGet]
        public async Task<IActionResult> AbrirEdicionRuta(int rutaID)
        {
            GestorConexionApis objgestor = new GestorConexionApis();
            List<RutaModel> lstresultado = await objgestor.ListarRutas();
            RutaModel encontrado = lstresultado.FirstOrDefault(u => u.rutaID == rutaID);

            List<UsuarioModel> listaUsuarios = await objgestor.ListarUsuarios();

            // Filtrar solo los supervisores
            var supervisores = listaUsuarios
                .Where(u => u.tipoUsuarioID == 2)
                .Select(u => new SelectListItem
                {
                    Value = u.usuarioID.ToString(),
                    Text = $"{u.nombre} {u.apellido}" // Mostrar nombre y apellido en el dropdown
                }).ToList();

            ViewBag.Supervisores = supervisores;

            return View(encontrado);
        }


        #endregion

        #region Acciones de Mantenimiento sobre RutaModel

        [HttpPost]
        public async Task<IActionResult> GuardarRuta(RutaModel ruta)
        {
            GestorConexionApis objgestor = new GestorConexionApis();
            var resultado = await objgestor.AgregarRuta(ruta);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Ruta agregada exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al agregar la ruta.";
            }
            return RedirectToAction("DataTableRuta");
        }


        [HttpPost]
        public async Task<IActionResult> EditarRuta(RutaModel ruta)
        {
            
            GestorConexionApis objgestor = new GestorConexionApis();
            var resultado = await objgestor.ModificarRuta(ruta);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Ruta modificada exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al modificar la ruta.";
            }
            return RedirectToAction("DataTableRuta");
        }

        [HttpGet]
        public async Task<IActionResult> BorrarRuta(int rutaID)
        {
            
            GestorConexionApis objgestor = new GestorConexionApis();
            var resultado = await objgestor.EliminarRuta(rutaID);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Ruta eliminada exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar la ruta.";
            }
            return RedirectToAction("DataTableRuta");
        }

        public async Task<List<SelectListItem>> GetRutasSelectList()
        {
            GestorConexionApis objconexion = new GestorConexionApis();
            List<RutaModel> resultado = await objconexion.ListarRutas();

            var rutasSelectList = resultado.Select(ruta => new SelectListItem
            {
                Value = ruta.rutaID.ToString(),
                Text = ruta.nombreRuta
            }).ToList();

            return rutasSelectList;
        }
        #endregion
    }
}
