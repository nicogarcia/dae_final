using System;
using System.Collections.Generic;
using AccesoDatos.Repos;
using AccesoDatos.UnitOfWork;
using Dominio.Entidades;
using Dominio.Queries;
using Dominio.Queries.Implementation;
using Dominio.Repos;
using Dominio.UnitOfWork;
using Dominio.Validacion;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using Presentacion.Controllers;
using System.Web.Mvc;
using Presentacion.Models;
using Presentacion.Models.Conversores;
using ReservasDCICTest.App_Start;

namespace ReservasDCICTest
{
    [TestClass]
    public class RecursosControllerTest
    {
        private readonly MoqMockingKernel _kernel;

        private Mock<IRecursosRepo> recursosRepoMock;
        private Mock<IConversorRecurso> conversorRecursoMock;
        
        public RecursosControllerTest()
        {
            NinjectWebCommon.Start();

            _kernel = ServiceLocator.Current.GetInstance<MoqMockingKernel>();

            LoadBindings(_kernel);
        }

        [TestInitialize]
        public void Setup()
        {
            // Reset kernel
            _kernel.Reset();

            // Create data
            var recurso = new Recurso("lab1", new TipoRecurso("Laboratorio"), "Laboratorio 1", "Descripcion lab1");

            CrearRecursosRepoMock(recurso);

            CrearConversorRecursosMock();

            CrearValidadorRecursosMock();

        }

        private void CrearValidadorRecursosMock()
        {
            var validadorDeRecursos = _kernel.GetMock<IValidadorDeRecursos>();

            validadorDeRecursos.Setup(mock => mock.EsValido(It.IsAny<Recurso>())).Returns(true);
            validadorDeRecursos.Setup(mock => mock.EsValidoParaActualizar(It.IsAny<Recurso>())).Returns(true);
            validadorDeRecursos.Setup(mock => mock.ObtenerErrores()).Returns(new Dictionary<string, string>());
        }

        private void CrearConversorRecursosMock()
        {
            // Create Mock
            conversorRecursoMock = _kernel.GetMock<IConversorRecurso>();

            // Setup conversion mocking
            conversorRecursoMock
                .Setup(mock => mock.CrearViewModel(It.IsAny<Recurso>()))
                .Returns((Recurso recurso) =>
                    new RecursoVM(
                        recurso.Id.ToString(),
                        recurso.Codigo, 
                        recurso.Nombre, 
                        recurso.Descripcion,
                        recurso.Tipo.Id.ToString())
                );
        }

        private void CrearRecursosRepoMock(Recurso recurso)
        {
            // Create RecursosRepo Mock
            recursosRepoMock = _kernel.GetMock<IRecursosRepo>();

            // Mock repo's methods
            recursosRepoMock.Setup(mock => mock.ObtenerPorId(It.IsAny<int>())).Returns(recurso);
            recursosRepoMock.Setup(mock => mock.Actualizar(It.IsAny<Recurso>()));
            recursosRepoMock.Setup(mock => mock.Todos()).Returns(new List<Recurso> {recurso});
        }

        [TestMethod]
        public void RecursosController_Details_Returns_Expected_Data()
        {
            var controller = _kernel.Get<RecursosController>();

            var result = controller.Details(1) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("lab1", ((Recurso)result.Model).Codigo);
        }

        [TestMethod]
        public void RecursosController_Editar_Caracteristica_Recurso_Con_Datos_Correctos()
        {
            var controller = _kernel.Get<RecursosController>();

            var result = controller.Edit(1) as ViewResult;

            var recursoVM = result.Model as RecursoVM;

            recursoVM.Descripcion = "Mock description";

            controller.Edit(recursoVM);

            recursosRepoMock.Verify(mock => mock.Actualizar(It.IsAny<Recurso>()), Times.Once());
        }
        
        public static void LoadBindings(IKernel kernel)
        {
            // These bindings are from modules but without scope specified
            // TODO: Refactor if possible to avoid code duplication

            // Acceso a datos
            //Kernel.Bind<IReservasContext>().To<ReservasContext>();

            //      Repos
            //kernel.Bind<IRecursosRepo>().To<RecursosRepo>();
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
            // kernel.Bind<IValidadorDeRecursos>().To<ValidadorDeRecursos>();
            kernel.Bind<IValidadorDeUsuarios>().To<ValidadorDeUsuarios>();
            kernel.Bind<IValidadorDeReserva>().To<ValidadorDeReserva>();

            //      Queries
            kernel.Bind<IRecursosQueriesTS>().To<RecursosQueriesTS>();
            kernel.Bind<IUsuariosQueriesTS>().To<UsuariosQueriesTS>();
            kernel.Bind<IReservasQueriesTS>().To<ReservasQueriesTS>();
            kernel.Bind<IMultipleTypeQueriesTS>().To<MultipleQueriesTS>();

            // Presentacion

            //      Conversores
            // kernel.Bind<IConversorRecurso>().To<ConversorRecurso>();
            kernel.Bind<IConversorReserva>().To<ConversorReserva>();
            kernel.Bind<IConversorUsuario>().To<ConversorUsuario>();


            // Tests
            kernel.Bind<RecursosController>().ToSelf();
        }    
    }
}

