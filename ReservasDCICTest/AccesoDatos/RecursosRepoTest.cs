using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AccesoDatos;
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
using Presentacion.Models.Conversores;
using ReservasDCICTest.App_Start;

namespace ReservasDCICTest.AccesoDatos
{
    [TestClass]
    public class RecursosRepoTest
    {
        public MoqMockingKernel Kernel;
        private Mock<IReservasContext> dbContextMock;

        public RecursosRepoTest()
        {
            NinjectWebCommon.Start();

            Kernel = ServiceLocator.Current.GetInstance<MoqMockingKernel>();

            TestMess.LoadBindings(Kernel);
        }

        [TestInitialize]
        public void Setup()
        {
            // Reset kernel
            Kernel.Reset();

            var data = new List<Recurso>
            {
                new Recurso("lab1", new TipoRecurso("Laboratorio"), "Laboratorio 1", "Descripcion lab1"),
                new Recurso("lab2", new TipoRecurso("Laboratorio"), "Laboratorio 2", "Descripcion lab2")
            }.AsQueryable();

            // Mock the DbSet
            var recursosDbSetMock = Kernel.GetMock<IDbSet<Recurso>>();
            recursosDbSetMock.Setup(mock => mock.Provider).Returns(data.Provider);
            recursosDbSetMock.Setup(mock => mock.Expression).Returns(data.Expression);
            recursosDbSetMock.Setup(mock => mock.ElementType).Returns(data.ElementType);
            recursosDbSetMock.Setup(mock => mock.GetEnumerator()).Returns(data.GetEnumerator());

            // Mock the Context
            dbContextMock = Kernel.GetMock<IReservasContext>();

            // Set the context's DbSet to the mocked one
            dbContextMock.Setup(mock => mock.Recursos).Returns(recursosDbSetMock.Object);
        }

        [TestMethod]
        public void RecursosRepo_Existe_Codigo_Retorna_Verdadero()
        {
            var recursosRepo = Kernel.Get<IRecursosRepo>();

            bool resultado = recursosRepo.ExisteCodigo("lab1");

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void RecursosRepo_Existe_Codigo_Retorna_Falso()
        {
            var recursosRepo = Kernel.Get<IRecursosRepo>();

            bool resultado = recursosRepo.ExisteCodigo("laboratorio1");

            Assert.IsFalse(resultado);
        }
    }
}
