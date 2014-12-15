using AccesoDatos;
using AccesoDatos.Repos;
using AccesoDatos.UnitOfWork;
using Dominio.Queries;
using Dominio.Queries.Implementation;
using Dominio.Repos;
using Dominio.UnitOfWork;
using Dominio.Validacion;
using Ninject;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using Ninject.Modules;
using Ninject.Web.Common;
using Presentacion.Controllers;
using Presentacion.Models.Conversores;

namespace ReservasDCICTest
{
    class TestMess
    {

        public static void LoadBindings(IKernel kernel)
        {
            // These bindings are from modules but without scope specified
            // TODO: Refactor if possible to avoid code duplication

            // Acceso a datos
            //Kernel.Bind<IReservasContext>().To<ReservasContext>();

            //      Repos
            kernel.Bind<IRecursosRepo>().To<RecursosRepo>();
            kernel.Bind<IUsuariosRepo>().To<UsuariosRepo>();
            kernel.Bind<IReservasRepo>().To<ReservasRepo>();
            kernel.Bind<ITiposDeCaracteristicasRepo>().To<TiposDeCaracteristicasRepo>();
            kernel.Bind<ITiposDeRecursosRepo>().To<TiposDeRecursosRepo>();

            //      UoW
            kernel.Bind<IUnitOfWork>().To<EFUnitOfWork>();

            // Dominio

            //      UoW
            kernel.Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>().InSingletonScope();

            //      Validadores
            kernel.Bind<IValidadorDeRecursos>().To<ValidadorDeRecursos>();
            kernel.Bind<IValidadorDeUsuarios>().To<ValidadorDeUsuarios>();
            kernel.Bind<IValidadorDeReserva>().To<ValidadorDeReserva>();

            //      Queries
            kernel.Bind<IRecursosQueriesTS>().To<RecursosQueriesTS>();
            kernel.Bind<IUsuariosQueriesTS>().To<UsuariosQueriesTS>();
            kernel.Bind<IReservasQueriesTS>().To<ReservasQueriesTS>();
            kernel.Bind<IMultipleTypeQueriesTS>().To<MultipleQueriesTS>();

            // Presentacion
            kernel.Bind<IConversorRecurso>().To<ConversorRecurso>();
            kernel.Bind<IConversorReserva>().To<ConversorReserva>();
            kernel.Bind<IConversorUsuario>().To<ConversorUsuario>();


            // Tests
            kernel.Bind<RecursosController>().ToMock();
        }    
    }

    class TestsModule : NinjectModule
    {
        
        public override void Load()
        {
            // These bindings are from modules but without scope specified
            // TODO: Refactor if possible to avoid code duplication
            
            // Acceso a datos
            Bind<IReservasContext>().To<ReservasContext>();


            /*
            //      Repos
            Bind<IRecursosRepo>().To<RecursosRepo>();
            Bind<IUsuariosRepo>().To<UsuariosRepo>();
            Bind<IReservasRepo>().To<ReservasRepo>();
            Bind<ITiposDeCaracteristicasRepo>().To<TiposDeCaracteristicasRepo>();
            Bind<ITiposDeRecursosRepo>().To<TiposDeRecursosRepo>();

            //      UoW
            Bind<IUnitOfWork>().To<EFUnitOfWork>();

            // Dominio
            
            //      UoW
            Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>().InSingletonScope();
            
            //      Validadores
            Bind<IValidadorDeRecursos>().To<ValidadorDeRecursos>();
            Bind<IValidadorDeUsuarios>().To<ValidadorDeUsuarios>();
            Bind<IValidadorDeReserva>().To<ValidadorDeReserva>();

            //      Queries
            Bind<IRecursosQueriesTS>().To<RecursosQueriesTS>();
            Bind<IUsuariosQueriesTS>().To<UsuariosQueriesTS>();
            Bind<IReservasQueriesTS>().To<ReservasQueriesTS>();
            Bind<IMultipleTypeQueriesTS>().To<MultipleQueriesTS>();

            // Presentacion
            Bind<IConversorRecurso>().To<ConversorRecurso>();
            Bind<IConversorReserva>().To<ConversorReserva>();
            Bind<IConversorUsuario>().To<ConversorUsuario>();


            // Tests
            Bind<RecursosController>().ToMock();
             * */
        }
    }
}
