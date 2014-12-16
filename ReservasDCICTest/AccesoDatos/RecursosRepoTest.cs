using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AccesoDatos;
using AccesoDatos.Repos;
using Dominio.Entidades;
using Dominio.Repos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ReservasDCICTest.AccesoDatos
{
    [TestClass]
    public class RecursosRepoTest
    {
        Mock<IReservasContext> dbContextMock;
        IRecursosRepo recursosRepoSUT;

        IList<Recurso> recursosAlmacenados;
        
        [TestInitialize]
        public void Setup()
        {
            recursosAlmacenados = new List<Recurso>
            {
                new Recurso("lab1", new TipoRecurso("Laboratorio"), "Laboratorio 1", "Descripcion lab1"),
                new Recurso("lab2", new TipoRecurso("Laboratorio"), "Laboratorio 2", "Descripcion lab2")
            };
            
            var data = recursosAlmacenados.AsQueryable();
            
            // Mock the DbSet
            var recursosDbSetMock = new Mock<IDbSet<Recurso>>();
            recursosDbSetMock.Setup(mock => mock.Provider).Returns(data.Provider);
            recursosDbSetMock.Setup(mock => mock.Expression).Returns(data.Expression);
            recursosDbSetMock.Setup(mock => mock.ElementType).Returns(data.ElementType);
            recursosDbSetMock.Setup(mock => mock.GetEnumerator()).Returns(data.GetEnumerator());

            // Mock the Context
            dbContextMock = new Mock<IReservasContext>();

            // Set the context's DbSet to the mocked one
            dbContextMock.Setup(mock => mock.Recursos).Returns(recursosDbSetMock.Object);

            recursosRepoSUT = new RecursosRepo(dbContextMock.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {
            dbContextMock = null;
            recursosRepoSUT = null;
        }

        [TestMethod]
        public void RecursosRepo_ExisteCodigo_Codigo_Existente_Retorna_Verdadero()
        {
            // Arrange

            //Act
            bool resultado = recursosRepoSUT.ExisteCodigo("lab1");

            // Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void RecursosRepo_ExisteCodigo_Codigo_Inexistente_Retorna_Falso()
        {
            // Arrange

            //Act
            bool resultado = recursosRepoSUT.ExisteCodigo("laboratorio1");
            
            // Assert
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void RecursosRepo_ExisteNombre_Nombre_Existente_Retorna_Verdadero()
        {
            // Arrange

            //Act
            bool resultado = recursosRepoSUT.ExisteNombre("Laboratorio 1");

            // Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void RecursosRepo_ExisteNombre_Nombre_Inexistente_Retorna_Falso()
        {
            // Arrange

            //Act
            bool resultado = recursosRepoSUT.ExisteNombre("oirotarobal 1");

            // Assert
            Assert.IsFalse(resultado);
        }
    }
}
