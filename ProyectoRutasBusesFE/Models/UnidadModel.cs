using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRutasBusesFE.Models
{
    public class UnidadModel
    {
        #region Propiedades

        public int unidadID { get; set; }

        [Required]
        [Display(Name = "Número de Placa")]
        public string numeroPlaca { get; set; }

        [Required]
        [Display(Name = "Tipo de Motor")]
        public string tipoMotor { get; set; }

        [Required]
        [Display(Name = "Capacidad")]
        public int capacidad { get; set; }

        [Required]
        [Display(Name = "Estado de la Unidad")]
        public string estadoUnidad { get; set; }

        [Display(Name = "ID de la Ruta")]
        public int? rutaID { get; set; }

        #endregion

        #region Constructor 

        public UnidadModel()
        {
            unidadID = 0;
            numeroPlaca = string.Empty;
            tipoMotor = string.Empty;
            capacidad = 0;
            estadoUnidad = string.Empty;
            rutaID = null;
        }

        #endregion
    }
}
