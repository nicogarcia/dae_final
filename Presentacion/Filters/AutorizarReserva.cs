using System.Web.Mvc;
using Dominio;
using Dominio.Entidades;
using Dominio.Repos;
using WebMatrix.WebData;

namespace Presentacion.Filters
{
    public class AutorizarReserva : ActionFilterAttribute
    {
    }

    public class FiltroAutorizar : IActionFilter
    {
        IUsuariosRepo UsuariosRepo;
        IReservasRepo ReservasRepo;

        public FiltroAutorizar(IUsuariosRepo usuariosRepo, IReservasRepo reservasRepo)
        {
            UsuariosRepo = usuariosRepo;
            ReservasRepo = reservasRepo;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string id = filterContext.ActionParameters["id"].ToString();

            var reserva = ReservasRepo.ObtenerPorId(int.Parse(id));

            if (reserva == null)
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }

            bool authorized = AutorizarSobreReserva(reserva);

            if (!authorized)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        private bool AutorizarSobreReserva(Reserva reserva)
        {
            var usuario = UsuariosRepo.BuscarUsuario(WebSecurity.CurrentUserName);

            if (usuario == null)
                return false;

            if (usuario.Tipo == TipoDeUsuario.Administrador)
                return true;

            if (usuario.Tipo == TipoDeUsuario.Miembro && reserva.Responsable == usuario)
                return true;

            return false;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}