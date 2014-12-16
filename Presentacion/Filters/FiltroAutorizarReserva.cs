using System.Web.Mvc;
using Dominio.Entidades;
using Dominio.Repos;
using Presentacion.Models;

namespace Presentacion.Filters
{
    public class FiltroAutorizarReserva : IActionFilter
    {
        IUsuariosRepo UsuariosRepo;
        IReservasRepo ReservasRepo;
        IWebSecuritySimpleWrapper WebSecurityWrapper;

        public FiltroAutorizarReserva(IUsuariosRepo usuariosRepo, IReservasRepo reservasRepo, IWebSecuritySimpleWrapper webSecurityWrapper)
        {
            UsuariosRepo = usuariosRepo;
            ReservasRepo = reservasRepo;
            WebSecurityWrapper = webSecurityWrapper;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string id;

            if (filterContext.ActionParameters.ContainsKey("id"))
                id = filterContext.ActionParameters["id"].ToString();
            else
                id = ((ReservaVM) filterContext.ActionParameters["reservaVM"]).Id.ToString();

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
            var usuario = UsuariosRepo.BuscarUsuario(WebSecurityWrapper.GetCurrentUsername());

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