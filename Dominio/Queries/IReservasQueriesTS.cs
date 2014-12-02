using System;
using System.Collections.Generic;
using Dominio.Entidades;

namespace Dominio.Queries
{
    public interface IReservasQueriesTS
    {

        IList<Reserva> BuscarReservas(string fechaDesde, string fechaHasta, string codigoRecurso,
            string usuarioResponsable, string estadoReserva);

    }
}
