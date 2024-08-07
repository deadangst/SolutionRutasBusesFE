using Microsoft.EntityFrameworkCore;
using ProyectoRutasBusesFE.Models;

namespace ProyectoRutasBusesFE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UsuarioModel> Usuarios { get; set; }
    }
}
