using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRutasBusesFE.Models
{
    public class RutaModel
    {
        #region Propiedades

        public int rutaID { get; set; }

        [Required]
        [Display(Name = "Nombre de la Ruta")]
        public string nombreRuta { get; set; }

        [Required]
        [Display(Name = "ID del Supervisor")]
        public int supervisorID { get; set; }

        [Required]
        [Display(Name = "Precio del Pasaje")]
        public decimal precioPasaje { get; set; }

        [Required]
        [Display(Name = "Cantidad de Paradas")]
        public int cantidadParadas { get; set; } // Nueva propiedad

        #endregion

        #region Constructor 

        public RutaModel()
        {
            rutaID = 0;
            nombreRuta = string.Empty;
            supervisorID = 0;
            precioPasaje = 0.0m;
            cantidadParadas = 0; // Inicializa la nueva propiedad
        }

        #endregion
    }
}
