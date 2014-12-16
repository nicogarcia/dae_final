
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Dominio.Entidades;
using Dominio.Repos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Presentacion.Filters;

namespace ReservasDCICTest.Presentacion
{
    [TestClass]
    public class FiltroAutorizarReservaTest
    {
        Mock<IUsuariosRepo> usuariosRepoMock;
        Mock<IReservasRepo> reservasRepoMock;
        Mock<ActionExecutingContext> actionExecutingContextMock;

        FiltroAutorizarReserva filtroAutorizarSUT;

        [TestInitialize]
        public void Setup()
        {
            // Setup data
            var usuario = new Usuario("messi", "Lionel", "Messi", "23424", "4365",
                    "messi@messi.com", "434545", TipoDeUsuario.Administrador);
            
            var recurso = new Recurso("lab1", new TipoRecurso("Laboratorio"), "Laboratorio 1", "Descripcion lab1");

            var reserva = new Reserva(usuario, usuario, recurso, new DateTime(), new DateTime().AddHours(1), "fasdf");

            // Mock Usuarios repo
            usuariosRepoMock = new Mock<IUsuariosRepo>();

            usuariosRepoMock.Setup(mock => mock.BuscarUsuario(It.IsAny<string>()))
                .Returns(usuario);

            // Mock Reservas repo
            reservasRepoMock = new Mock<IReservasRepo>();
            reservasRepoMock.Setup(mock => mock.ObtenerPorId(It.IsAny<int>()))
                .Returns(reserva);

            // Mock Action context
            actionExecutingContextMock = new Mock<ActionExecutingContext>();

            actionExecutingContextMock.SetupAllProperties();
            actionExecutingContextMock.Object.ActionParameters = new Dictionary<string, object>();
            
            // Mock WebSecurity
            var webSecurityMock = new Mock<IWebSecuritySimpleWrapper>();
            webSecurityMock.Setup(mock => mock.GetCurrentUsername()).Returns("nothing");

            // Setup SUT
            filtroAutorizarSUT = new FiltroAutorizarReserva(usuariosRepoMock.Object,
                reservasRepoMock.Object, webSecurityMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            reservasRepoMock = null;
            usuariosRepoMock = null;
            filtroAutorizarSUT = null;
            actionExecutingContextMock = null;
        }

        [TestMethod]
        public void FiltroAutorizarReserva_Reserva_Inexistente_Retorna_No_Encontrado()
        {
            // Arrange
            reservasRepoMock.Setup(mock => mock.ObtenerPorId(It.IsAny<int>()))
                .Returns((Reserva) null);

            actionExecutingContextMock.Object.ActionParameters.Add("id", 0);

            // Act
            filtroAutorizarSUT.OnActionExecuting(actionExecutingContextMock.Object);

            // Assert
            Assert.IsInstanceOfType(actionExecutingContextMock.Object.Result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void FiltroAutorizarReserva_Reserva_Existente_Usuario_Incorrecto_Retorna_No_Autorizado()
        {
            // Arrange
            var usuario = new Usuario("eldiego", "Diego", "Maradona", "23424", "4365",
                    "diego@diego.com", "434545", TipoDeUsuario.Miembro);
            
            usuariosRepoMock.Setup(mock => mock.BuscarUsuario(It.IsAny<string>())).Returns(usuario);

            actionExecutingContextMock.Object.ActionParameters.Add("id", 0);

            // Act
            filtroAutorizarSUT.OnActionExecuting(actionExecutingContextMock.Object);

            // Assert
            Assert.IsInstanceOfType(actionExecutingContextMock.Object.Result, typeof(HttpUnauthorizedResult));
        }

        [TestMethod]
        public void FiltroAutorizarReserva_Reserva_Existente_Usuario_Correcto_Retorna_Autorizado()
        {
            // Arrange
            actionExecutingContextMock.Object.ActionParameters.Add("id", 0);

            // Act
            filtroAutorizarSUT.OnActionExecuting(actionExecutingContextMock.Object);

            // Assert
            Assert.IsNull(actionExecutingContextMock.Object.Result);
        }

        [TestMethod]
        public void FiltroAutorizarReserva_Reserva_Existente_Usuario_Administrador_Retorna_No_Autorizado()
        {
            // Arrange
            var usuario = new Usuario("eldiego", "Diego", "Maradona", "23424", "4365",
                    "diego@diego.com", "434545", TipoDeUsuario.Administrador);

            usuariosRepoMock.Setup(mock => mock.BuscarUsuario(It.IsAny<string>())).Returns(usuario);

            actionExecutingContextMock.Object.ActionParameters.Add("id", 0);

            // Act
            filtroAutorizarSUT.OnActionExecuting(actionExecutingContextMock.Object);

            // Assert
            Assert.IsNull(actionExecutingContextMock.Object.Result);
        }
    }
}
