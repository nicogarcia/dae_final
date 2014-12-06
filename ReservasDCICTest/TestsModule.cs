using Dominio.Queries;
using Dominio.Repos;
using Dominio.UnitOfWork;
using Dominio.Validacion;
using Ninject.MockingKernel;
using Ninject.Modules;
using Presentacion.Controllers;
using Presentacion.Models.Conversores;

namespace ReservasDCICTest
{
    class TestsModule : NinjectModule
    {
        
        public override void Load()
        {
            /*Bind<ITiposDeCaracteristicasRepo>().ToMock();
            Bind<ITiposDeRecursosRepo>().ToMock();
            Bind<IUnitOfWorkFactory>().ToMock();
            Bind<IConversorRecurso>().ToMock();
            Bind<IValidadorDeRecursos>().ToMock();
            Bind<IRecursosQueriesTS>().ToMock();
            Bind<IMultipleTypeQueriesTS>().ToMock();*/

            Bind<RecursosController>().ToMock();
        }
    }
}
