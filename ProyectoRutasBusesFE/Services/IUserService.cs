using System.Security.Claims;
using System.Threading.Tasks;
using ProyectoRutasBusesFE.Models;

namespace ProyectoRutasBusesFE.Services
{
    public interface IUserService
    {
        Task<UsuarioModel> GetUserAsync(ClaimsPrincipal principal);
    }
}
