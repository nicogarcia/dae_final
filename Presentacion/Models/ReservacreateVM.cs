using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models
{
    public class ReservacreateVM
    {
        public bool administrador;

        public ReservacreateVM(bool adminstrador)
        {
            this.administrador = adminstrador;
        }

        public int Id { get; private set; }


        public string Responsable { get; set; }

        [Required]
        public DateTime Inicio { get; set; }

        [Required]
        public DateTime Fin { get; set; }

        [Required]
        [MaxLength(50)]
        public string Descripcion { get; set; }

        [Required]
        public string RecursoReservado { get; set; }
    }
}