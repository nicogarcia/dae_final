using AccesoDatos.UnitOfWork;
using Dominio.Repos;
using AccesoDatos.Repos;
using Dominio.UnitOfWork;
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
            Bind<ITiposDeCaracteristicasRepo>().To<TiposDeCaracteristicasRepo>().InRequestScope();
            Bind<ITiposDeRecursosRepo>().To<TiposDeRecursosRepo>().InRequestScope();

            Bind<IUnitOfWork>().To<EFUnitOfWork>().InRequestScope();
            Bind<IReservaRepo>().To<ReservaRepo>().InRequestScope();
        }
    }
}
