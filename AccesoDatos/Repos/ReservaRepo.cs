using Dominio;
using Dominio.Repos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccesoDatos.Repos
{
    class ReservaRepo : RepoBase<Reserva>, IReservaRepo
    {

        public ReservaRepo(ReservasContext reservasContext) : base(reservasContext)
        {
        }

        public IList<Reserva> buscarReservas(string fechaDesde, string fechaHasta, string codigoRecurso,
            string usuarioResponsable, string estadoReserva)
        {
            // Query de recursos
            IQueryable<Reserva> queryReservas = Ctx.Reservas;
            
            // TODO: FIX DATE FILTER
            
            // Aplicar filtros
            if (!string.IsNullOrEmpty(fechaDesde))
                queryReservas = queryReservas.Where(r => r.Inicio.ToString() == fechaDesde);
            
            if (!string.IsNullOrEmpty(fechaHasta))
                queryReservas = queryReservas.Where(r => r.Fin.ToString() == fechaHasta);
            
            if (!string.IsNullOrEmpty(codigoRecurso))
                queryReservas = queryReservas.Where(r => r.RecursoReservado.Codigo == codigoRecurso);
            
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

        public bool ExisteReserva(string codigo_recurso, DateTime inicio, DateTime fin)
        {
            var existeReserva = Ctx.Reservas.Any(reserva =>
                (reserva.Inicio >= inicio && reserva.Fin > inicio) &&
                (reserva.Fin <= fin && fin > reserva.Inicio) &&
                (reserva.RecursoReservado.Codigo == codigo_recurso));

            return existeReserva;
        }
 
    }
}
