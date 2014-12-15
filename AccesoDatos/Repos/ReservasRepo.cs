using Dominio;
using Dominio.Entidades;
using Dominio.Repos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccesoDatos.Repos
{
    public class ReservasRepo : RepoBase<Reserva>, IReservasRepo
    {

        public ReservasRepo(IReservasContext reservasContext)
            : base(reservasContext)
        {
        }

        public bool ExisteReserva(string codigoRecurso, DateTime inicio, DateTime fin)
        {
            var existeReserva = Ctx.Reservas.Any(reserva =>
                (reserva.Inicio >= inicio && reserva.Fin > inicio) &&
                (reserva.Fin <= fin && fin > reserva.Inicio) &&
                (reserva.RecursoReservado.Codigo == codigoRecurso));

            return existeReserva;
        }
    }
}
