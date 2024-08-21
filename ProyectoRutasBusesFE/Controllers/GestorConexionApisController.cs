using ProyectoRutasBusesFE.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProyectoRutasBusesFE.Controllers
{
    public class GestorConexionApis
    {

        #region Propiedades

        public HttpClient ConexionApis { get; set; }

        #endregion

        #region Constructor

        public GestorConexionApis()
        {
            ConexionApis = new HttpClient();
            EstablecerDatosBaseConexion();
        }

        #endregion

        #region Metodos

        #region Privado

        private void EstablecerDatosBaseConexion()
        {
        
            ConexionApis.BaseAddress = new Uri("http://localhost:85/");
            //ConexionApis.BaseAddress = new Uri("http://localhost:22482/");
            ConexionApis.DefaultRequestHeaders.Accept.Clear();
            ConexionApis.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion

        #region Publico

        #region Autorizaciones

        public async Task<List<PerfilModel>> AutorizacionesPorUsuarios(UsuarioModel P_Entidad)
        {
            List<PerfilModel> listaresultado = new List<PerfilModel>();
            string rutaAPI = @"api/RutaBuses/AutorizacionesPorUsuarios";

            ConexionApis.DefaultRequestHeaders.Add("pEmail", P_Entidad.email);

            HttpResponseMessage resultadoconsumo = await ConexionApis.GetAsync(rutaAPI);
            if (resultadoconsumo.IsSuccessStatusCode)
            {
                string jsonstring = await resultadoconsumo.Content.ReadAsStringAsync();
                listaresultado = JsonSerializer.Deserialize<List<PerfilModel>>(jsonstring);
            }

            return listaresultado;
        }

        #endregion

        #region Usuarios

        public async Task<List<UsuarioModel>> ListarUsuarios()
        {
            List<UsuarioModel> listaresultado = new List<UsuarioModel>();
            string rutaAPI = @"api/RutaBuses/ConsultarUsuarios";

            HttpResponseMessage resultadoconsumo = await ConexionApis.GetAsync(rutaAPI);
            if (resultadoconsumo.IsSuccessStatusCode)
            {
                string jsonstring = await resultadoconsumo.Content.ReadAsStringAsync();
                listaresultado = JsonSerializer.Deserialize<List<UsuarioModel>>(jsonstring);
            }

            return listaresultado;
        }

        public async Task<bool> AgregarUsuario(UsuarioModel usuario)
        {
            string rutaAPI = @"api/RutaBuses/AgregarUsuario";

            HttpResponseMessage resultadoconsumo = await ConexionApis.PostAsJsonAsync(rutaAPI, usuario);
            return resultadoconsumo.IsSuccessStatusCode;
        }

        public async Task<bool> ModificarUsuario(UsuarioModel usuario)
        {
            string rutaAPI = @"api/RutaBuses/ModificarUsuario";

            ConexionApis.DefaultRequestHeaders.Add("usuarioID", usuario.usuarioID.ToString());
            HttpResponseMessage resultadoconsumo = await ConexionApis.PutAsJsonAsync(rutaAPI, usuario);
            return resultadoconsumo.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarUsuario(int usuarioID)
        {
            string rutaAPI = @"api/RutaBuses/EliminarUsuario";

            ConexionApis.DefaultRequestHeaders.Add("usuarioID", usuarioID.ToString());
            HttpResponseMessage resultadoconsumo = await ConexionApis.DeleteAsync(rutaAPI);
            return resultadoconsumo.IsSuccessStatusCode;
        }

        public async Task<UsuarioModel> ObtenerUsuarioPorEmail(string email)
        {
            UsuarioModel usuario = null;
            string rutaAPI = $"api/RutaBuses/ObtenerUsuarioPorEmail?email={email}";

            HttpResponseMessage resultadoconsumo = await ConexionApis.GetAsync(rutaAPI);
            if (resultadoconsumo.IsSuccessStatusCode)
            {
                string jsonstring = await resultadoconsumo.Content.ReadAsStringAsync();
                usuario = JsonSerializer.Deserialize<UsuarioModel>(jsonstring);
            }

            return usuario;
        }

        #endregion

        #region Choferes

        public async Task<List<ChoferModel>> ListarChoferes()
        {
            List<ChoferModel> listaresultado = new List<ChoferModel>();
            string rutaAPI = @"api/RutaBuses/ConsultarChoferes";

            HttpResponseMessage resultadoconsumo = await ConexionApis.GetAsync(rutaAPI);
            if (resultadoconsumo.IsSuccessStatusCode)
            {
                string jsonstring = await resultadoconsumo.Content.ReadAsStringAsync();
                listaresultado = JsonSerializer.Deserialize<List<ChoferModel>>(jsonstring);
            }

            return listaresultado;
        }

        public async Task<bool> AgregarChofer(ChoferModel chofer)
        {
            string rutaAPI = @"api/RutaBuses/AgregarChofer";

            HttpResponseMessage resultadoconsumo = await ConexionApis.PostAsJsonAsync(rutaAPI, chofer);
            return resultadoconsumo.IsSuccessStatusCode;
        }

        public async Task<bool> ModificarChofer(ChoferModel chofer)
        {
            string rutaAPI = @"api/RutaBuses/ModificarChofer";

            ConexionApis.DefaultRequestHeaders.Add("choferID", chofer.choferID.ToString());
            HttpResponseMessage resultadoconsumo = await ConexionApis.PutAsJsonAsync(rutaAPI, chofer);
            return resultadoconsumo.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarChofer(int choferID)
        {
            string rutaAPI = @"api/RutaBuses/EliminarChofer";

            ConexionApis.DefaultRequestHeaders.Add("choferID", choferID.ToString());
            HttpResponseMessage resultadoconsumo = await ConexionApis.DeleteAsync(rutaAPI);
            return resultadoconsumo.IsSuccessStatusCode;
        }

        #endregion

        #region Unidades

        public async Task<List<UnidadModel>> ListarUnidades()
        {
            List<UnidadModel> listaresultado = new List<UnidadModel>();
            string rutaAPI = @"api/RutaBuses/ConsultarUnidades";

            HttpResponseMessage resultadoconsumo = await ConexionApis.GetAsync(rutaAPI);
            if (resultadoconsumo.IsSuccessStatusCode)
            {
                string jsonstring = await resultadoconsumo.Content.ReadAsStringAsync();
                listaresultado = JsonSerializer.Deserialize<List<UnidadModel>>(jsonstring);
            }

            return listaresultado;
        }

        public async Task<bool> AgregarUnidad(UnidadModel unidad)
        {
            string rutaAPI = @"api/RutaBuses/AgregarUnidad";

            HttpResponseMessage resultadoconsumo = await ConexionApis.PostAsJsonAsync(rutaAPI, unidad);
            return resultadoconsumo.IsSuccessStatusCode;
        }

        public async Task<bool> ModificarUnidad(UnidadModel unidad)
        {
            string rutaAPI = @"api/RutaBuses/ModificarUnidad";

            ConexionApis.DefaultRequestHeaders.Add("unidadID", unidad.unidadID.ToString());
            HttpResponseMessage resultadoconsumo = await ConexionApis.PutAsJsonAsync(rutaAPI, unidad);
            return resultadoconsumo.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarUnidad(int unidadID)
        {
            string rutaAPI = @"api/RutaBuses/EliminarUnidad";

            ConexionApis.DefaultRequestHeaders.Add("unidadID", unidadID.ToString());
            HttpResponseMessage resultadoconsumo = await ConexionApis.DeleteAsync(rutaAPI);
            return resultadoconsumo.IsSuccessStatusCode;
        }

        #endregion

        #region Rutas

        public async Task<List<RutaModel>> ListarRutas()
        {
            List<RutaModel> listaresultado = new List<RutaModel>();
            string rutaAPI = @"api/RutaBuses/ConsultarRutas";

            HttpResponseMessage resultadoconsumo = await ConexionApis.GetAsync(rutaAPI);
            if (resultadoconsumo.IsSuccessStatusCode)
            {
                string jsonstring = await resultadoconsumo.Content.ReadAsStringAsync();
                listaresultado = JsonSerializer.Deserialize<List<RutaModel>>(jsonstring);
            }

            return listaresultado;
        }

        public async Task<bool> AgregarRuta(RutaModel ruta)
        {
            string rutaAPI = @"api/RutaBuses/AgregarRuta";

            HttpResponseMessage resultadoconsumo = await ConexionApis.PostAsJsonAsync(rutaAPI, ruta);
            return resultadoconsumo.IsSuccessStatusCode;
        }

        public async Task<bool> ModificarRuta(RutaModel ruta)
        {
            string rutaAPI = @"api/RutaBuses/ModificarRuta";

            ConexionApis.DefaultRequestHeaders.Add("rutaID", ruta.rutaID.ToString());
            HttpResponseMessage resultadoconsumo = await ConexionApis.PutAsJsonAsync(rutaAPI, ruta);
            return resultadoconsumo.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarRuta(int rutaID)
        {
            string rutaAPI = @"api/RutaBuses/EliminarRuta";

            ConexionApis.DefaultRequestHeaders.Add("rutaID", rutaID.ToString());
            HttpResponseMessage resultadoconsumo = await ConexionApis.DeleteAsync(rutaAPI);
            return resultadoconsumo.IsSuccessStatusCode;
        }

        #endregion

        #region Recaudaciones

        public async Task<List<RecaudacionModel>> ListarRecaudaciones()
        {
            List<RecaudacionModel> listaresultado = new List<RecaudacionModel>();
            string rutaAPI = @"api/RutaBuses/ConsultarRecaudaciones";

            HttpResponseMessage resultadoconsumo = await ConexionApis.GetAsync(rutaAPI);
            if (resultadoconsumo.IsSuccessStatusCode)
            {
                string jsonstring = await resultadoconsumo.Content.ReadAsStringAsync();
                listaresultado = JsonSerializer.Deserialize<List<RecaudacionModel>>(jsonstring);
            }

            return listaresultado;
        }

        public async Task<bool> AgregarRecaudacion(RecaudacionModel recaudacion)
        {
            string rutaAPI = @"api/RutaBuses/AgregarRecaudacion";

            HttpResponseMessage resultadoconsumo = await ConexionApis.PostAsJsonAsync(rutaAPI, recaudacion);
            return resultadoconsumo.IsSuccessStatusCode;
        }

        public async Task<List<RecaudacionModel>> ConsultarRecaudacionesPorRutaYFecha(int rutaID, DateTime fecha)
        {
            List<RecaudacionModel> listaresultado = new List<RecaudacionModel>();
            string rutaAPI = $"api/RutaBuses/ConsultarRecaudacionesPorRutaYFecha?rutaID={rutaID}&fecha={fecha.ToString("yyyy-MM-dd")}";

            HttpResponseMessage resultadoconsumo = await ConexionApis.GetAsync(rutaAPI);
            if (resultadoconsumo.IsSuccessStatusCode)
            {
                string jsonstring = await resultadoconsumo.Content.ReadAsStringAsync();
                listaresultado = JsonSerializer.Deserialize<List<RecaudacionModel>>(jsonstring);
            }

            return listaresultado;
        }

        // Nuevo método para validar si una recaudación ya existe
        public async Task<bool> ValidarRecaudacionExistente(int rutaID, int unidadID, DateTime fecha)
        {
            string rutaAPI = $"api/RutaBuses/ValidarRecaudacionExistente?rutaID={rutaID}&unidadID={unidadID}&fecha={fecha.ToString("yyyy-MM-dd")}";

            HttpResponseMessage resultadoconsumo = await ConexionApis.GetAsync(rutaAPI);
            if (resultadoconsumo.IsSuccessStatusCode)
            {
                string jsonstring = await resultadoconsumo.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(jsonstring);
            }

            return false;
        }

        #endregion


        #region Bitacora

        public async Task<List<BitacoraModel>> ListarBitacora()
        {
            List<BitacoraModel> listaresultado = new List<BitacoraModel>();
            string rutaAPI = @"api/RutaBuses/ConsultarBitacora";

            HttpResponseMessage resultadoconsumo = await ConexionApis.GetAsync(rutaAPI);
            if (resultadoconsumo.IsSuccessStatusCode)
            {
                string jsonstring = await resultadoconsumo.Content.ReadAsStringAsync();
                listaresultado = JsonSerializer.Deserialize<List<BitacoraModel>>(jsonstring);
            }

            return listaresultado;
        }

        #endregion

        #endregion

        #endregion
    }
}
