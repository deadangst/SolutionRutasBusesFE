using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRutasBusesFE.Models
{
    public class RecaudacionModel
    {
        #region Propiedades

        public int recaudacionID { get; set; }

        [Required]
        [Display(Name = "ID de la Ruta")]
        public int rutaID { get; set; }

        [Required]
        [Display(Name = "ID de la Unidad")]
        public int unidadID { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime fecha { get; set; }

        [Required]
        [Display(Name = "Cantidad de Pasajeros")]
        public int cantidadPasajeros { get; set; }

        [Required]
        [Display(Name = "Monto Recaudado")]
        public decimal montoRecaudado { get; set; }

        #endregion

        #region Constructor 

        public RecaudacionModel()
        {
            recaudacionID = 0;
            rutaID = 0;
            unidadID = 0;
            fecha = DateTime.MinValue;
            cantidadPasajeros = 0;
            montoRecaudado = 0.0M;
        }

        #endregion
    }
}
