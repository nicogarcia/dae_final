using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentacion.Models
{
    public class ReservaVM
    {
        public int Id { get; set; }

        public UsuarioVM Creador { get; set; }

        public UsuarioVM Responsable { get; set; }

        public DateTime Inicio { get; set; }

        public DateTime Fin { get; set; }

        public DateTime FechaCreacion { get; set; }
        
        [MaxLength(50)]
        public string Descripcion { get; set; }

        public RecursoVM RecursoReservado { get; set; }

        public string Estado { get; set; }
    }
}