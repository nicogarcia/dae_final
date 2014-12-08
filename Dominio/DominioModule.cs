using Dominio.Queries;
using Dominio.Queries.Implementation;
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
            Bind<IValidadorDeUsuarios>().To<ValidadorDeUsuarios>().InRequestScope();
            Bind<IValidadorDeReserva>().To<ValidadorDeReserva>().InRequestScope();
            
            Bind<IRecursosQueriesTS>().To<RecursosQueriesTS>();
            Bind<IUsuariosQueriesTS>().To<UsuariosQueriesTS>();
            Bind<IReservasQueriesTS>().To<ReservasQueriesTS>();
            Bind<IMultipleTypeQueriesTS>().To<MultipleQueriesTS>();

        }
    }
}
