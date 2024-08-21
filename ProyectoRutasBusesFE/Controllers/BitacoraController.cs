using Microsoft.AspNetCore.Mvc;
using ProyectoRutasBusesFE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoRutasBusesFE.Controllers
{
    public class BitacoraController : Controller
    {
        private readonly GestorConexionApis _gestorConexionApis;

        public BitacoraController(GestorConexionApis gestorConexionApis)
        {
            _gestorConexionApis = gestorConexionApis;
        }

        public async Task<IActionResult> Index()
        {
            GestorConexionApis objconexion = new GestorConexionApis();
            List<BitacoraModel> resultado = await objconexion.ListarBitacora();
            List<UsuarioModel> listaUsuarios = await objconexion.ListarUsuarios();

            // Crear un diccionario para mapear usuarioID a nombre de usuario
            var usuariosDiccionario = listaUsuarios
                .ToDictionary(u => u.usuarioID, u => $"{u.nombre} {u.apellido}");

            // Agregar una entrada para Super Usuario
            usuariosDiccionario[0] = "Super Usuario";

            ViewBag.Usuarios = usuariosDiccionario;

            return View(resultado);
        }


        public IActionResult FiltrarListaBitacora()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FiltrarListaBitacora(DateTime? fechaEventoBuscar)
        {
            // Validación del campo de búsqueda por fecha
            if (!fechaEventoBuscar.HasValue)
            {
                TempData["ErrorMessage"] = "Debe seleccionar una fecha.";
                return RedirectToAction("Index");
            }

            List<BitacoraModel> listaBitacora = await _gestorConexionApis.ListarBitacora();

            if (fechaEventoBuscar.HasValue)
                listaBitacora = listaBitacora.Where(item => item.fechaAccion.Date == fechaEventoBuscar.Value.Date).ToList();

            // Obtener la lista de usuarios
            List<UsuarioModel> listaUsuarios = await _gestorConexionApis.ListarUsuarios();

            // Crear un diccionario para mapear usuarioID a nombre de usuario
            var usuariosDiccionario = listaUsuarios
                .ToDictionary(u => u.usuarioID, u => $"{u.nombre} {u.apellido}");

            // Agregar una entrada para Super Usuario
            usuariosDiccionario[0] = "Super Usuario";

            ViewBag.Usuarios = usuariosDiccionario;

            return View("Index", listaBitacora);
        }

    }
}
