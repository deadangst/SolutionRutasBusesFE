using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRutasBusesFE.Models
{
    public class PerfilModel
    {
        #region Propiedades

        public int codigoPerfil { get; set; }
        [Required]
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool estado { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string email { get; set; }

        #endregion

        #region Constructor

        public PerfilModel()
        {
            codigoPerfil = 0;
            descripcion = string.Empty;
            estado = false;
            email = string.Empty;
        }

        #endregion
    }
}
