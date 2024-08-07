using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProyectoRutasBusesFE.Controllers
{
    public class ConfigController : Controller
    {
        [Authorize]
        public IActionResult UserProfile()
        {
            // Aquí puedes agregar la lógica para obtener los detalles del perfil del usuario y pasarlos a la vista
            // Por simplicidad, estamos creando un objeto de ejemplo
            var userProfile = new
            {
                Nombre = User.Identity.Name,
                Email = User.FindFirst(ClaimTypes.Email).Value,
                TipoUsuario = User.IsInRole("SuperAdmin") ? "Super Administrador" :
                              User.IsInRole("Supervisor") ? "Supervisor" :
                              User.IsInRole("Mecanico") ? "Mecánico" :
                              "Chofer"
            };

            return View(userProfile);
        }
    }
}
