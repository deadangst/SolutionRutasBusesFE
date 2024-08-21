using System.Security.Claims;
using System.Threading.Tasks;
using ProyectoRutasBusesFE.Controllers;
using ProyectoRutasBusesFE.Models;

namespace ProyectoRutasBusesFE.Services
{
    public class UserService : IUserService
    {
        private readonly GestorConexionApis _gestorConexionApis;

        public UserService(GestorConexionApis gestorConexionApis)
        {
            _gestorConexionApis = gestorConexionApis;
        }

        public async Task<UsuarioModel> GetUserAsync(ClaimsPrincipal principal)
        {
            var email = principal?.Identity?.Name;
            if (email == null)
                return null;

            return await _gestorConexionApis.ObtenerUsuarioPorEmail(email);
        }
    }
}
