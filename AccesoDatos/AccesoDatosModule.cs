using Dominio.Repos;
using Ninject.Modules;
using Ninject.Web.Common;

namespace AccesoDatos
{
    public class AccesoDatosModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUsuariosRepo>().To<UsuariosRepo>().InRequestScope(); 
            Bind<ReservasContext>().ToSelf().InRequestScope();
        }
    }
}
