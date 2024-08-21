using Microsoft.AspNetCore.Mvc;
using ProyectoRutasBusesFE.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoRutasBusesFE.Controllers
{
    [Authorize]
    public class ChoferController : Controller
    {
        #region Acciones de apertura de vistas

        public async Task<IActionResult> Index()
        {
            GestorConexionApis objconexion = new GestorConexionApis();
            List<ChoferModel> resultado = await objconexion.ListarChoferes();
            List<UsuarioModel> listaUsuarios = await objconexion.ListarUsuarios();
            List<RutaModel> listaRutas = await objconexion.ListarRutas();
            List<UnidadModel> listaUnidades = await objconexion.ListarUnidades();

            // Filtrar solo los choferes
            var usuarios = listaUsuarios
                .Where(u => u.tipoUsuarioID == 4)
                .ToDictionary(u => u.usuarioID, u => $"{u.nombre} {u.apellido}");

            var rutas = listaRutas
                .ToDictionary(r => r.rutaID, r => r.nombreRuta);

            var unidades = listaUnidades
                .ToDictionary(u => u.unidadID, u => u.numeroPlaca);

            ViewBag.Usuarios = usuarios;
            ViewBag.Rutas = rutas;
            ViewBag.Unidades = unidades;

            return View(resultado);
        }

        public async Task<IActionResult> AbrirCrearChofer()
        {
            GestorConexionApis objgestor = new GestorConexionApis();
            List<UsuarioModel> listaUsuarios = await objgestor.ListarUsuarios();
            List<RutaModel> listaRutas = await objgestor.ListarRutas();
            List<UnidadModel> listaUnidades = await objgestor.ListarUnidades();

            // Filtrar solo los choferes
            var usuarios = listaUsuarios
                .Where(u => u.tipoUsuarioID == 4)
                .Select(u => new SelectListItem
                {
                    Value = u.usuarioID.ToString(),
                    Text = $"{u.nombre} {u.apellido}" // Mostrar nombre y apellido en el dropdown
                }).ToList();

            var rutas = listaRutas
                .Select(r => new SelectListItem
                {
                    Value = r.rutaID.ToString(),
                    Text = r.nombreRuta
                }).ToList();

            var unidades = listaUnidades
                .Select(u => new SelectListItem
                {
                    Value = u.unidadID.ToString(),
                    Text = u.numeroPlaca
                }).ToList();

            ViewBag.Usuarios = usuarios;
            ViewBag.Rutas = rutas;
            ViewBag.Unidades = unidades;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FiltrarListaChoferes(string ChoferBuscar)
        {
            GestorConexionApis objgestor = new GestorConexionApis();
            List<ChoferModel> listachofer = await objgestor.ListarChoferes();
            List<UsuarioModel> listaUsuarios = await objgestor.ListarUsuarios();
            List<RutaModel> listaRutas = await objgestor.ListarRutas();
            List<UnidadModel> listaUnidades = await objgestor.ListarUnidades();

            if (!string.IsNullOrEmpty(ChoferBuscar))
                listachofer = listachofer.FindAll(item => item.numeroEmpleado.Contains(ChoferBuscar)).ToList();

            // Filtrar solo los choferes
            var usuarios = listaUsuarios
                .Where(u => u.tipoUsuarioID == 4)
                .ToDictionary(u => u.usuarioID, u => $"{u.nombre} {u.apellido}");

            var rutas = listaRutas
                .ToDictionary(r => r.rutaID, r => r.nombreRuta);

            var unidades = listaUnidades
                .ToDictionary(u => u.unidadID, u => u.numeroPlaca);

            ViewBag.Usuarios = usuarios;
            ViewBag.Rutas = rutas;
            ViewBag.Unidades = unidades;

            return View("Index", listachofer);
        }

        [HttpGet]
        public async Task<IActionResult> AbrirEdicionChofer(int choferID)
        {
            GestorConexionApis objgestor = new GestorConexionApis();
            List<ChoferModel> lstresultado = await objgestor.ListarChoferes();
            ChoferModel encontrado = lstresultado.FirstOrDefault(u => u.choferID == choferID);

            List<UsuarioModel> listaUsuarios = await objgestor.ListarUsuarios();
            List<RutaModel> listaRutas = await objgestor.ListarRutas();
            List<UnidadModel> listaUnidades = await objgestor.ListarUnidades();

            // Filtrar solo los choferes
            var usuarios = listaUsuarios
                .Where(u => u.tipoUsuarioID == 4)
                .Select(u => new SelectListItem
                {
                    Value = u.usuarioID.ToString(),
                    Text = $"{u.nombre} {u.apellido}" // Mostrar nombre y apellido en el dropdown
                }).ToList();

            var rutas = listaRutas
                .Select(r => new SelectListItem
                {
                    Value = r.rutaID.ToString(),
                    Text = r.nombreRuta
                }).ToList();

            var unidades = listaUnidades
                .Select(u => new SelectListItem
                {
                    Value = u.unidadID.ToString(),
                    Text = u.numeroPlaca
                }).ToList();

            ViewBag.Usuarios = usuarios;
            ViewBag.Rutas = rutas;
            ViewBag.Unidades = unidades;

            return View(encontrado);
        }

        #endregion

        #region Acciones de Mantenimiento sobre ChoferModel

        [HttpPost]
        public async Task<IActionResult> GuardarChofer(ChoferModel chofer)
        {
            GestorConexionApis objgestor = new GestorConexionApis();
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
            GestorConexionApis objgestor = new GestorConexionApis();
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
            GestorConexionApis objgestor = new GestorConexionApis();
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
