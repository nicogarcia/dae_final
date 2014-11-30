using System.Collections.Generic;
using System.Web.Mvc;

namespace Presentacion.Models
{
    public interface IBusquedaReservaVM
    {
        IEnumerable<SelectListItem> SelectEstadosDeReserva();
        IEnumerable<SelectListItem> SelectDeRecursos();
        IEnumerable<SelectListItem> SelectDeUsuarios();
    }
}