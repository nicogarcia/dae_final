using Dominio.Entidades;
using Dominio.Repos;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using Presentacion.Controllers;
using System.Web.Mvc;
using ReservasDCICTest.App_Start;

namespace ReservasDCICTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly MoqMockingKernel _kernel;

        public UnitTest1()
        {
            NinjectWebCommon.Start();

            _kernel = ServiceLocator.Current.GetInstance<MoqMockingKernel>();
        }

        [TestMethod]
        public void RecursosRepoTest()
        {
            var recurso = new Recurso("lab1", new TipoRecurso("Laboratorio"), "Laboratorio 1", "Descripcion lab1");

            var recursosRepo = _kernel.GetMock<IRecursosRepo>();
            recursosRepo.Setup(mock => mock.ObtenerPorId(It.IsAny<int>())).Returns(recurso);

            var controller = _kernel.Get<RecursosController>();

            ActionResult result = controller.Details(1);

            Assert.AreEqual("lab1", ((Recurso) ((ViewResult) result).Model).Codigo);

        }
    }
}

