using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dominio;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Presentacion.Models
{
    public class UsuarioVM
    {
        public int id { get; private set; }

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



        public IEnumerable<SelectListItem> SelectoTipoDeUsuario
        {
            get
            {
                var items = new List<SelectListItem>();
                var valores = Enum.GetValues(typeof(TipoDeUsuario));
                foreach (var tu in valores)
                {
                    items.Add(new SelectListItem() { Text = tu.ToString(), Value = tu.GetHashCode().ToString() });
                }

                return items;
            }
            set { }
        }

        public IEnumerable<SelectListItem> Filtros
        {
            get
            {
                var items = new List<SelectListItem>();
                SelectListItem filtro = new SelectListItem() { Text = "Nombre", Value = "Nombre".GetHashCode().ToString() };
                items.Add(filtro);
                filtro = new SelectListItem() { Text = "Apellido", Value = "Apellido".GetHashCode().ToString() };
                items.Add(filtro);
                filtro = new SelectListItem() { Text = "Legajo", Value = "Legajo".GetHashCode().ToString() };
                items.Add(filtro);
                return items;
            }
            set { }
        }

    }
}