using Dominio;
using Dominio.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Repos
{
    class ReservaRepo : RepoBase<Reserva>, IReservaRepo
    {
        public ReservaRepo(ReservasContext reservasContext) : base(reservasContext) { }


        public Reserva CrearReserva(string usuario_creado, string usuario_responsable, string codigo_recurso, DateTime fecha_desde, DateTime fecha_hasta, string descripcion)
        {
            UsuariosRepo usuariorepo = new UsuariosRepo(Ctx);
            Usuario creador = usuariorepo.BuscarUsuario(usuario_creado);
            Usuario responsanble = usuariorepo.BuscarUsuario(usuario_responsable);
            RecursosRepo recursorepo = new RecursosRepo(Ctx);
            Recurso recurso = recursorepo.BuscarRecurso(codigo_recurso);


            return null;
        }

        public bool ExisteResponsable(string usuario_responsable)
        {
            UsuariosRepo usuariorepo = new UsuariosRepo(Ctx);
            Usuario responsable = usuariorepo.BuscarUsuario(usuario_responsable);
            if (responsable == null)
            {
                return false;
            }
            return true;
        }
        public bool ExisteRecurso(string codigo_recurso)
        {
            RecursosRepo recursorepo = new RecursosRepo(Ctx);
            Recurso recurso = recursorepo.BuscarRecurso(codigo_recurso);
            if (recurso == null)
            {
                return false;
            }
            return true;
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
            IQueryable<Reserva> listado_reservas = Ctx.Reservas;
            listado_reservas = listado_reservas.Where(r => r.Inicio >= inicio && r.Fin>inicio);
            listado_reservas = listado_reservas.Where(r => r.Fin <= fin && fin > r.Inicio );
            listado_reservas = listado_reservas.Where(r => r.RecursoReservado.Codigo == codigo_recurso);
            if(listado_reservas.ToList().Count > 0)
            {
                return true;
            }
            return false;
        
        }
 
    }
}
