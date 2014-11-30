using System;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models
{
    public class ReservaVM
    {
        public int Id { get; set; }

        public string Creador { get; set; }

        public string Responsable { get; set; }

        [Required]
        public DateTime Inicio { get; set; }

        [Required]
        public DateTime Fin { get; set; }

        public DateTime FechaCreacion { get; set; }

        [Required]
        [MaxLength(50)]
        public string Descripcion { get; set; }

        [Required]
        public string RecursoReservado { get; set; }

        public string Estado { get; set; }

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