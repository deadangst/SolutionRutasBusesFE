using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using ProyectoRutasBusesFE.Controllers;
using ProyectoRutasBusesFE.Services;  // Agrega esta línea
using System;

namespace ProyectoRutasBusesFE
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Registrar RutaController como un servicio
            services.AddScoped<RutaController>();
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Autenticacion/Index";
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    option.AccessDeniedPath = "/Autenticacion/AccesoDenegado";
                });

            services.AddHttpContextAccessor(); // Agrega esta línea
            services.AddScoped<GestorConexionApis>();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>(); // Agrega esta línea
            services.AddSession(); // Agrega esto si aún no está en tu proyecto
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession(); // Agrega esta línea

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Autenticacion}/{action=Index}/{id?}");
            });
        }
    }
}
