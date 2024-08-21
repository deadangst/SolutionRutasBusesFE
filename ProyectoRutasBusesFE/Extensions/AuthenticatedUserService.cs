using Microsoft.AspNetCore.Http;
using ProyectoRutasBusesFE.Extensions;
using ProyectoRutasBusesFE.Models;

namespace ProyectoRutasBusesFE.Services
{
    public interface IAuthenticatedUserService
    {
        UsuarioModel GetAuthenticatedUser();
    }

    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UsuarioModel GetAuthenticatedUser()
        {
            return _httpContextAccessor.HttpContext.Session.GetObject<UsuarioModel>("UsuarioAutenticado");
        }
    }
}
