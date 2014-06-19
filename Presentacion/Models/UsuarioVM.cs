using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dominio;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models
{
    public class UsuarioVM
    {
        public int Id { get; private set; }

        [Required]
        public TipoCaracteristica Tipo { get; set; } //Tipo de usuario Administrador/Miembro
        [Required]
        public TipoCaracteristica EstadoUsuario { get; set; } //Estado de usuario
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
        public string DNI { get; set; }//DNI*: 9 dígitos

        [MaxLength(4)]
        public string Legajo { get; set; }//Legajo: 4 dígitos

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }//Email*: 50 caracters

        [MaxLength(20)]
        public string Telefono { get; set; }//Teléfono: 20 caracteres



        public IEnumerable<TipoDeUsuario> SelectoTipoDeUsuario { get; private set; }

    }
}