using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Presentacion.Models
{
    public class ReservaVM
    {
        public int Id { get; set; }

        public string Creador { get; set; }

        public string Responsable { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")] 
        [Required]
        public DateTime Inicio { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")] 
        [Required]
        public DateTime Fin { get; set; }

        public DateTime FechaCreacion { get; set; }

        [Required]
        [MaxLength(50)]
        public string Descripcion { get; set; }

        [Required]
        public string RecursoReservado { get; set; }

        public string Estado { get; set; }

        public List<SelectListItem> SelectUsuarioResponsable { get; set; }

        public ReservaVM()
        {
            Inicio = DateTime.Today;
            Fin = DateTime.Today.AddDays(1);
        }

        public ReservaVM(string recursoReservado) : this()
        {
            RecursoReservado = recursoReservado;
        }
    }
}