using System.Collections.Generic;
using System.Web.Mvc;

namespace Presentacion.Models
{
    public class BusquedaReservasVM
    {
        public IList<ReservaVM> ListaDeReservas { set; get; }

        public IList<SelectListItem> SelectRecursos;

        public IList<SelectListItem> SelectEstadoReserva;

        public IList<SelectListItem> SelectUsuarios;

    }
}