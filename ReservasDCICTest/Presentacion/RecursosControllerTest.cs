using System.Collections.Generic;
using System.Web.Mvc;
using Dominio.Entidades;
using Dominio.Queries;
using Dominio.Repos;
using Dominio.UnitOfWork;
using Dominio.Validacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Presentacion.Controllers;
using Presentacion.Models;
using Presentacion.Models.Conversores;

namespace ReservasDCICTest.Presentacion
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

            // Mock Recursos repo
            CrearRecursosRepoMock(recurso);

            // Mock Conversor de recursos
            CrearConversorRecursosMock();

            // Mock Validador de recursos
            CrearValidadorRecursosMock();

            // Mock UoW factory
            uowFactoryMock = new Mock<IUnitOfWorkFactory>();
            uowFactoryMock.SetupGet(mock => mock.Actual).Returns(new Mock<IUnitOfWork>().Object);

            // Mock tipos de caracteristicas repo
            tiposDeCaracteristicasRepoMock = new Mock<ITiposDeCaracteristicasRepo>();
            tiposDeRecursosRepoMock = new Mock<ITiposDeRecursosRepo>();

            // Mock Queries
            recursosQueriesMock = new Mock<IRecursosQueriesTS>();
            multipleQueriesMock = new Mock<IMultipleTypeQueriesTS>();

            // Initialize SUT
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
            recursosRepoMock.Setup(mock => mock.Eliminar(It.IsAny<Recurso>()));
            recursosRepoMock.Setup(mock => mock.Todos()).Returns(new List<Recurso> { recurso });
        }

        [TestMethod]
        public void RecursosController_Detalle_Recurso_Existente()
        {
            // Arrange
            var id = 0;

            // Act
            var result = recursosControllerSUT.Details(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("lab1", ((Recurso)result.Model).Codigo);
        }

        [TestMethod]
        public void RecursosController_Detalle_Recurso_No_Existente()
        {
            // Arrange
            var id = 100;
            recursosRepoMock.Setup(mock => mock.ObtenerPorId(It.IsAny<int>())).Returns((Recurso)null);

            // Act
            var result = recursosControllerSUT.Details(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
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

        [TestMethod]
        public void RecursosController_Eliminar_Recurso_Existente_Elimina_Correctamente()
        {
            // Arrange
            var id = 1;
            var recurso = recursosRepoMock.Object.ObtenerPorId(id);
            var result = recursosControllerSUT.Delete(id) as ViewResult;

            // Act
            recursosControllerSUT.DeleteConfirmed(id);

            // Assert
            Assert.IsNotInstanceOfType(result, typeof(HttpNotFoundResult));
            Assert.AreEqual(recurso.EstadoActual, EstadoRecurso.Inactivo);
            recursosRepoMock.Verify(mock => mock.Actualizar(It.IsAny<Recurso>()), Times.Once());
        }

        [TestMethod]
        public void RecursosController_Eliminar_Recurso_Inxistente_Retorna_No_Encontrado()
        {
            // Arrange
            var idInexistente = 15;
            recursosRepoMock.Setup(mock => mock.ObtenerPorId(It.IsAny<int>())).Returns((Recurso) null);
            
            // Act
            var result = recursosControllerSUT.Delete(idInexistente);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
            recursosRepoMock.Verify(mock => mock.Actualizar(It.IsAny<Recurso>()), Times.Never());
        }
    }
}

