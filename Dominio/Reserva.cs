using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dominio.Repos;

namespace Dominio
{
    public class Reserva
    {
        public int Id { get; private set; }

        [Required]
        public virtual Usuario  Creador { get; set; }

        [Required]
        public virtual Usuario Responsable { get; set; }

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
        public virtual Recurso RecursoReservado { get; set; }


        public EstadoReserva Estado { get; set; }

        public Reserva()
        { }

        public Reserva (Usuario usuario_creador, Usuario usuario_responsable, Recurso recurso_reservado, DateTime inicio, DateTime fin, string descripcion)
        {
           
            Creador =  usuario_creador;
            Responsable = usuario_responsable;
            RecursoReservado = recurso_reservado;
            Inicio = inicio;
            Fin = fin;
            Descripcion = descripcion;
            Estado = EstadoReserva.Activo;
            FechaCreacion = DateTime.Now;

        }
    }
}
