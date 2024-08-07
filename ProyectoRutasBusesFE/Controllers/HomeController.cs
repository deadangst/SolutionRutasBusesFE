using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoRutasBusesFE.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoRutasBusesFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Authorize]
        public IActionResult Index()
        {
            // Obtener información del usuario autenticado
            var userName = User.Identity.Name;
            ViewBag.UserName = userName;

            // Aquí deberías obtener las rutas (organizaciones) que el usuario tiene permiso para ver
            // Por simplicidad, creamos una lista de ejemplo
            var rutas = new List<string> { "Ruta 1", "Ruta 2", "Ruta 3" };
            ViewBag.Rutas = rutas;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
