using System;
using System.Collections.Generic;
using Dominio.Entidades;
using Dominio.Queries;
using Dominio.Repos;
using Dominio.UnitOfWork;
using Dominio.Validacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Presentacion.Controllers;
using System.Web.Mvc;
using Presentacion.Models;
using Presentacion.Models.Conversores;

namespace ReservasDCICTest
{
    [TestClass]
    public class RecursosControllerTest
    {

        private Mock<IRecursosRepo> recursosRepoMock;
        private Mock<IConversorRecurso> conversorRecursoMock;
        private Mock<IValidadorDeRecursos> validadorRecursosMock;

        private Mock<IUnitOfWorkFactory> uowFactoryMock;

        private Mock<ITiposDeCaracteristicasRepo> tiposDeCaracteristicasRepoMock;
        private Mock<ITiposDeRecursosRepo> tiposDeRecursosRepoMock;

        private Mock<IRecursosQueriesTS> recursosQueriesMock;
        private Mock<IMultipleTypeQueriesTS> multipleQueriesMock;

        private RecursosController recursosControllerSUT;

        [TestInitialize]
        public void Setup()
        {
            // Create data
            var recurso = new Recurso("lab1", new TipoRecurso("Laboratorio"), "Laboratorio 1", "Descripcion lab1");

            CrearRecursosRepoMock(recurso);

            CrearConversorRecursosMock();

            CrearValidadorRecursosMock();

            CrearUoWMockFactory();

            tiposDeCaracteristicasRepoMock = new Mock<ITiposDeCaracteristicasRepo>();
            tiposDeRecursosRepoMock = new Mock<ITiposDeRecursosRepo>();

            recursosQueriesMock = new Mock<IRecursosQueriesTS>();
            multipleQueriesMock = new Mock<IMultipleTypeQueriesTS>();

            recursosControllerSUT = new RecursosController(
                recursosRepoMock.Object,
                tiposDeCaracteristicasRepoMock.Object,
                tiposDeRecursosRepoMock.Object,
                uowFactoryMock.Object,
                conversorRecursoMock.Object,
                validadorRecursosMock.Object,
                recursosQueriesMock.Object,
                multipleQueriesMock.Object
            );
        }

        [TestCleanup]
        public void Cleanup()
        {
            recursosRepoMock = null;
            recursosQueriesMock = null;
            recursosControllerSUT = null;
            multipleQueriesMock = null;
            validadorRecursosMock = null;
            conversorRecursoMock = null;
            uowFactoryMock = null;
            tiposDeRecursosRepoMock = null;
            tiposDeCaracteristicasRepoMock = null;
        }

        private void CrearUoWMockFactory()
        {
            uowFactoryMock = new Mock<IUnitOfWorkFactory>();
            
            uowFactoryMock.SetupGet(mock => mock.Actual).Returns(new Mock<IUnitOfWork>().Object);
        }

        private void CrearValidadorRecursosMock()
        {
            validadorRecursosMock = new Mock<IValidadorDeRecursos>();

            validadorRecursosMock.Setup(mock => mock.EsValido(It.IsAny<Recurso>())).Returns(true);
            validadorRecursosMock.Setup(mock => mock.EsValidoParaActualizar(It.IsAny<Recurso>())).Returns(true);
            validadorRecursosMock.Setup(mock => mock.ObtenerErrores()).Returns(new Dictionary<string, string>());
        }

        private void CrearConversorRecursosMock()
        {
            // Create Mock
            conversorRecursoMock = new Mock<IConversorRecurso>();

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

            conversorRecursoMock
                .Setup(mock => mock.ActualizarDomainModel(It.IsAny<RecursoVM>()))
                .Returns((RecursoVM recursoVM) => 
                    new Recurso(
                        recursoVM.Codigo,
                        new TipoRecurso("Laboratorio 1"),
                        recursoVM.Nombre,
                        recursoVM.Descripcion
                    )
                );
        }

        private void CrearRecursosRepoMock(Recurso recurso)
        {
            // Create RecursosRepo Mock
            recursosRepoMock = new Mock<IRecursosRepo>();

            // Mock repo's methods
            recursosRepoMock.Setup(mock => mock.ObtenerPorId(It.IsAny<int>())).Returns(recurso);
            recursosRepoMock.Setup(mock => mock.Actualizar(It.IsAny<Recurso>()));
            recursosRepoMock.Setup(mock => mock.Todos()).Returns(new List<Recurso> { recurso });
        }

        [TestMethod]
        public void RecursosController_Detalle_Recurso_Existente()
        {
            var id = 0;
            var result = recursosControllerSUT.Details(id) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("lab1", ((Recurso)result.Model).Codigo);
        }

        [TestMethod]
        public void RecursosController_Detalle_Recurso_No_Existente()
        {
            recursosRepoMock.Setup(mock => mock.ObtenerPorId(It.IsAny<int>())).Returns((Recurso) null);
            var id = 100;
            var result = recursosControllerSUT.Details(id) as ViewResult;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void RecursosController_Editar_Caracteristica_Recurso_Con_Datos_Correctos()
        {
            // Arrange
            var result = recursosControllerSUT.Edit(1) as ViewResult;
            var recursoVM = result.Model as RecursoVM;
            recursoVM.Descripcion = "Mock description";

            // Act
            recursosControllerSUT.Edit(recursoVM);

            // Assert
            recursosRepoMock.Verify(mock => mock.Actualizar(It.IsAny<Recurso>()), Times.Once());
        }

    }
}

