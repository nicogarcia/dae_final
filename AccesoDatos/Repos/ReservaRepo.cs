using Dominio;
using Dominio.Repos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccesoDatos.Repos
{
    class ReservaRepo : RepoBase<Reserva>, IReservaRepo
    {
        IUsuariosRepo UsuariosRepo;
        IRecursosRepo RecursosRepo;

        public ReservaRepo(ReservasContext reservasContext, IUsuariosRepo usuariosRepo, IRecursosRepo recursorepo) : base(reservasContext)
        {
            UsuariosRepo = usuariosRepo;
            RecursosRepo = recursorepo;
        }

      

        public IList<Reserva> buscarReservas(string fecha_desde, string fecha_hasta, string tipo_recurso,
            string usuario_responsable, string estado_reserva)
        {
            // Query de recursos
            IQueryable<Reserva> listado_reservas = Ctx.Reservas;

            // Aplicar filtros
            if (!string.IsNullOrEmpty(fecha_desde))
                listado_reservas = listado_reservas.Where(r => r.Inicio.ToString() == fecha_desde);
            
            if (!string.IsNullOrEmpty(fecha_hasta))
                listado_reservas = listado_reservas.Where(r => r.Fin.ToString() == fecha_hasta);
            
            if (!string.IsNullOrEmpty(tipo_recurso))
                listado_reservas = listado_reservas.Where(r => r.RecursoReservado.Tipo.ToString() == tipo_recurso);
            
            if (!string.IsNullOrEmpty(usuario_responsable))
                listado_reservas = listado_reservas.Where(r => r.Responsable.NombreUsuario.ToString() == usuario_responsable);

            if (!string.IsNullOrEmpty(estado_reserva))
                listado_reservas = listado_reservas.Where(r => r.Estado.ToString() == estado_reserva);

            return listado_reservas.ToList();                       
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
