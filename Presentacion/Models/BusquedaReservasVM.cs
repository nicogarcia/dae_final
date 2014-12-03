using Dominio;
using Dominio.Entidades;
using Dominio.Repos;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using Presentacion.Soporte;

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