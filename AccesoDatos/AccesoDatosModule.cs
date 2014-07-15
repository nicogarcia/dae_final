using AccesoDatos.Repos;
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

            Bind<IRecursosRepo>().To<RecursosRepo>().InRequestScope();
            Bind<ITiposDeCaracteriscasRepo>().To<TiposDeCaracteristicasRepo>().InRequestScope();
            Bind<ITiposDeRecursosRepo>().To<TiposDeRecursosRepo>().InRequestScope();

        }
    }
}
