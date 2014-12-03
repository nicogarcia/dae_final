using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Entidades;
using Dominio.Repos;

namespace Dominio.Queries.Implementation
{
    public class ReservasQueriesTS : IReservasQueriesTS
    {
        private IReservasRepo ReservasRepo;

        public ReservasQueriesTS(IReservasRepo reservasRepo)
        {
            ReservasRepo = reservasRepo;
        }

        public IList<Reserva> BuscarReservas(string fechaDesde, string fechaHasta, string codigoRecurso,
            string usuarioResponsable, string estadoReserva)
        {
            // Query de recursos
            IQueryable<Reserva> queryReservas = ReservasRepo.AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(fechaDesde) && !string.IsNullOrEmpty(fechaHasta))
            {
                var inicio = DateTime.Parse(fechaDesde);
                var fin = DateTime.Parse(fechaHasta);

                queryReservas = queryReservas
                    .Where(r => (r.Inicio >= inicio) && (r.Fin <= fin));
            }

            if (!string.IsNullOrEmpty(codigoRecurso))
                queryReservas = queryReservas.Where(r => r.RecursoReservado.Codigo.ToUpper().Contains(codigoRecurso));

            if (!string.IsNullOrEmpty(usuarioResponsable))
                queryReservas = queryReservas.Where(
                    r => r.Responsable.NombreUsuario.ToUpper().Contains(usuarioResponsable.ToUpper()));

            if (!string.IsNullOrEmpty(estadoReserva))
            {
                var estado = (EstadoReserva)Enum.Parse(typeof(EstadoReserva), estadoReserva);
                queryReservas = queryReservas.Where(r => r.Estado == estado);
            }

            return queryReservas.ToList();
        }

        public IList<Reserva> ReservasDelUsuario(string username)
        {
            return ReservasRepo.AsQueryable().Where(reserva => reserva.Creador.NombreUsuario == username).ToList();
        }

    }
}
