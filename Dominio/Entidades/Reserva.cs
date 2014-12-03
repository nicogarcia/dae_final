using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidades
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
        {
        }

        public Reserva (Usuario usuarioCreador, Usuario usuarioResponsable, Recurso recursoReservado, DateTime inicio, DateTime fin, string descripcion)
        {
            Creador =  usuarioCreador;
            Responsable = usuarioResponsable;
            RecursoReservado = recursoReservado;
            Inicio = inicio;
            Fin = fin;
            Descripcion = descripcion;
            Estado = EstadoReserva.Activo;
            FechaCreacion = DateTime.Now;
        }
    }
}
