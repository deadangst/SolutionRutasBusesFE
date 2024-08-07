using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRutasBusesFE.Models
{
    public class ChoferModel
    {
        #region Propiedades

        public int choferID { get; set; }

        [Required]
        [Display(Name = "Usuario ID")]
        public int usuarioID { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "Número de Empleado")]
        public string numeroEmpleado { get; set; }

        #endregion

        #region Constructor 

        public ChoferModel()
        {
            choferID = 0;
            usuarioID = 0;
            numeroEmpleado = string.Empty;
        }

        #endregion
    }
}
