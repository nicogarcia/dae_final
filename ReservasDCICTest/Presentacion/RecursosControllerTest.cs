using System;
using Dominio.Entidades;
using Dominio.Repos;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
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

        public RecursosControllerTest()
        {
            NinjectWebCommon.Start();

            _kernel = ServiceLocator.Current.GetInstance<MoqMockingKernel>();
        }

        [TestInitialize]
        public void Setup()
        {
            _kernel.Reset();
        }
        
        [TestMethod]
        public void RecursosController_Editar_Caracteristica_Recurso()
        {
            var recurso = new Recurso("lab1", new TipoRecurso("Laboratorio"), "Laboratorio 1", "Descripcion lab1");

            var recursosRepo = _kernel.GetMock<IRecursosRepo>();
            recursosRepo.Setup(mock => mock.ObtenerPorId(It.IsAny<int>())).Returns(recurso);
            recursosRepo.Setup(mock => mock.Actualizar(It.IsAny<Recurso>()));

            var recursoVMMock = new RecursoVM("1", "lab1", "Laboratorio 1", "Descripcion lab1", "1");

            var conversorRecurso = _kernel.GetMock<IConversorRecurso>();
            conversorRecurso.Setup(mock => mock.CrearViewModel(It.IsAny<Recurso>())).Returns(recursoVMMock);
            
            var controller = _kernel.Get<RecursosController>();

            ActionResult result = controller.Edit(1);
            
            var recursoVM = ((RecursoVM) ((ViewResult) result).Model);
            
            recursoVM.Descripcion = "Mock description";

            controller.Edit(recursoVM);
            
            recursosRepo.Verify(mock => mock.Actualizar(It.IsAny<Recurso>()), Times.Once());
        }
    }
}

