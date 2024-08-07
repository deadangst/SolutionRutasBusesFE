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
            List<BitacoraModel> resultado = await _gestorConexionApis.ListarBitacora();
            return View(resultado);
        }

        public IActionResult FiltrarListaBitacora()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FiltrarListaBitacora(DateTime? fechaEventoBuscar)
        {
            // Validación del campo de busqueda por fecha
            if (!fechaEventoBuscar.HasValue)
            {
                TempData["ErrorMessage"] = "Debe seleccionar una fecha.";
                return RedirectToAction("Index");
            }
            List<BitacoraModel> listaBitacora = await _gestorConexionApis.ListarBitacora();

            if (fechaEventoBuscar.HasValue)
                listaBitacora = listaBitacora.Where(item => item.fechaAccion.Date == fechaEventoBuscar.Value.Date).ToList();

            return View("Index", listaBitacora);
        }
    }
}
