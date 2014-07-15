using Dominio.UnitOfWork;
using Ninject.Modules;

namespace Dominio
{
    public class DominioModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>().InSingletonScope();
        }
    }
}
