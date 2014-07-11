using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dominio
{
    public class Reserva
    {
        public int Id { get; private set; }

        [Required]
        public Usuario Creador { get; set; }

        [Required]
        public Usuario Responsable { get; set; }

        [Required]
        public DateTime Inicio { get; set; }

        [Required]
        public DateTime Fin { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [MaxLength(50)]
        public string Descripcion { get; set; }

        [Required]
        public Recurso RecursoReservado { get; set; }


        public EstadoReserva Estado { get; set; }

        public Reserva()
        { }
    }
}
