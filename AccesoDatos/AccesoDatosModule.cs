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
            // Contexto de base de datos
            Bind<IReservasContext>().To<ReservasContext>().InRequestScope();

            // Repositorios
            Bind<IUsuariosRepo>().To<UsuariosRepo>().InRequestScope(); 
            Bind<IRecursosRepo>().To<RecursosRepo>().InRequestScope();
            Bind<IReservasRepo>().To<ReservasRepo>().InRequestScope();
            Bind<ITiposDeCaracteristicasRepo>().To<TiposDeCaracteristicasRepo>().InRequestScope();
            Bind<ITiposDeRecursosRepo>().To<TiposDeRecursosRepo>().InRequestScope();

            // Unit of Work
            Bind<IUnitOfWork>().To<EFUnitOfWork>().InRequestScope();
        }
    }
}
