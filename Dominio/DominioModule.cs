using Dominio.Autorizacion;
using Dominio.UnitOfWork;
using Dominio.Validacion;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Dominio
{
    public class DominioModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>().InSingletonScope();

            Bind<IValidadorDeRecursos>().To<ValidadorDeRecursos>().InRequestScope();

            Bind<IAutorizacionUsuario>().To<AutorizacionUsuario>().InRequestScope();

        }
    }
}
