using Dominio;
using Dominio.Entidades;
using Presentacion.Soporte;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Presentacion.Models
{
    public class UsuarioVM
    {
        public int id { get;  set; }

        [Required]
        public TipoDeUsuario Tipo { get; set; } //Tipo de usuario Administrador/Miembro

        [Required]
        [MaxLength(20)]
        public string NombreUsuario { get; set; } //Es el enlace con el sistema de autentificacion de "Presentación".

        [Required]
        [MinLength(6)]
        [MaxLength(18)]
        public  string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }//Nombre*: 50 caracteres

        [Required]
        [MaxLength(50)]
        public string Apellido { get; set; }//Apellido*: 50 caracteres

        [MaxLength(9)]
        [Phone]
        public string DNI { get; set; }//DNI*: 9 dígitos

        [MaxLength(4)]
        [Phone]
        public string Legajo { get; set; }//Legajo: 4 dígitos

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }//Email*: 50 caracters

        [MaxLength(20)]
        [Phone]
        public string Telefono { get; set; }//Teléfono: 20 caracteres

        public string EstadoUsuario { get; set; }
        public UsuarioVM(int id)
        {
            this.id = id;
        }

        public UsuarioVM()
        {
            // TODO: Complete member initialization
        }
        public IEnumerable<SelectListItem> SelectoTipoDeUsuario
        {
            get
            {
                return typeof(TipoDeUsuario).ToSelectList();
            }

            private set { }
        }
    }
}