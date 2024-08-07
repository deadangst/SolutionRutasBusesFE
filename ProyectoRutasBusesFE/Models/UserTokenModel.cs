using System;

namespace ProyectoRutasBusesFE.Models
{
    public class UserTokenModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
