using System.Collections.Generic;
using Dominio.Entidades;
using Dominio.Repos;
using Dominio.Validacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ReservasDCICTest.Dominio
{
    [TestClass]
    public class ValidadorRecursosTest
    {
        private Mock<IRecursosRepo> recursosRepoMock;
        private Mock<ITiposDeRecursosRepo> tiposDeRecursosRepoMock;
        private IValidadorDeRecursos validadorDeRecursosSUT;

        [TestInitialize]
        public void SetUp()
        {
            var recurso = new Recurso("lab1", new TipoRecurso("Laboratorio"), "Laboratorio 1", "Descripcion lab1");

            CrearRecursosRepoMock(recurso);

            CrearTiposDeRecursosMock();

            validadorDeRecursosSUT = new ValidadorDeRecursos(
                recursosRepoMock.Object,
                tiposDeRecursosRepoMock.Object
            );
        }

        private void CrearTiposDeRecursosMock()
        {
            tiposDeRecursosRepoMock = new Mock<ITiposDeRecursosRepo>();
            tiposDeRecursosRepoMock.Setup(mock => mock.ExisteId(It.IsAny<int>())).Returns(true);
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

        [TestCleanup]
        public void CleanUp()
        {
            recursosRepoMock = null;
            tiposDeRecursosRepoMock = null;
        }

        [TestMethod]
        public void ValidadorRecursos_CodigoExistente_Retorna_Invalido()
        {
            // Arrange
            var recurso = new Recurso("lab1", new TipoRecurso("Laboratorio"), "Lab N°1", "Descripcion Lab N°1");
            recursosRepoMock.Setup(mock => mock.ExisteCodigo(It.IsAny<string>())).Returns(true);

            // Act
            bool result = validadorDeRecursosSUT.EsValido(recurso);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidadorRecursos_Datos_Validos_Retorna_Valido()
        {
            // Arrange
            var recurso = new Recurso("lab1", new TipoRecurso("Laboratorio"), "Lab N°1", "Descripcion Lab N°1");
            
            recursosRepoMock.Setup(mock => mock.ExisteCodigo(It.IsAny<string>())).Returns(false);
            recursosRepoMock.Setup(mock => mock.ExisteNombre(It.IsAny<string>())).Returns(false);

            // Act
            bool result = validadorDeRecursosSUT.EsValido(recurso);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidadorRecursos_EsValidoParaActualizar_Retorna_Verdadero()
        {
            // Arrange
            var recurso = new Recurso("lab1", new TipoRecurso("Laboratorio"), "Lab N°1", "Descripcion Lab N°1");

            recursosRepoMock.Setup(mock => mock.ExisteCodigo(It.IsAny<string>())).Returns(true);
            recursosRepoMock.Setup(mock => mock.ExisteNombre(It.IsAny<string>())).Returns(true);

            // Act
            bool result = validadorDeRecursosSUT.EsValidoParaActualizar(recurso);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidadorRecursos_EsValidoParaActualizar_Ya_Existe_Codigo_Retorna_Falso()
        {
            // Arrange
            var recurso = new Recurso("lab1", new TipoRecurso("Laboratorio"), "Lab N°1", "Descripcion Lab N°1");

            recursosRepoMock.Setup(mock => mock
                .ExisteCodigoEnOtroRecurso(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            recursosRepoMock.Setup(mock => mock
                .ExisteNombreEnOtroRecurso(It.IsAny<string>(), It.IsAny<int>())).Returns(false);

            // Act
            bool result = validadorDeRecursosSUT.EsValidoParaActualizar(recurso);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidadorRecursos_EsValidoParaActualizar_Datos_Validos_Retorna_Verdadero()
        {
            // Arrange
            var recurso = new Recurso("lab1", new TipoRecurso("Laboratorio"), "Lab N°1", "Descripcion Lab N°1");

            recursosRepoMock.Setup(mock => mock
                .ExisteCodigoEnOtroRecurso(It.IsAny<string>(), It.IsAny<int>())).Returns(false);

            recursosRepoMock.Setup(mock => mock
                .ExisteNombreEnOtroRecurso(It.IsAny<string>(), It.IsAny<int>())).Returns(false);

            // Act
            bool result = validadorDeRecursosSUT.EsValidoParaActualizar(recurso);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
