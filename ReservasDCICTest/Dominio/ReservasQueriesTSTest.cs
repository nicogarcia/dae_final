using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Entidades;
using Dominio.Queries;
using Dominio.Queries.Implementation;
using Dominio.Repos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ReservasDCICTest.Dominio
{
    [TestClass]
    public class ReservasQueriesTSTest
    {
        private Mock<IReservasRepo> reservasRepoMock;
        private IReservasQueriesTS reservasQueriesSUT;

        private IList<Reserva> reservasAlmacenadas;

        [TestInitialize]
        public void Setup()
        {
            // Setup data
            var recurso1 = new Recurso("lab1", new TipoRecurso("Laboratorio"), "Laboratorio 1", "Descripcion lab1");
            var recurso2 = new Recurso("lab2", new TipoRecurso("Laboratorio"), "Laboratorio 2", "Descripcion lab2");

            var usuario1 = new Usuario("messi", "Lionel", "Messi", "23424", "4365",
                "messi@messi.com", "434545", TipoDeUsuario.Administrador);
            var usuario2 = new Usuario("ronaldo", "Cristiano", "Ronaldo", "234234", "4365",
                "cristiano@ronaldo.com", "434545", TipoDeUsuario.Administrador);

            reservasAlmacenadas = new List<Reserva>
            {
                new Reserva(usuario1, usuario1, recurso1, new DateTime(), new DateTime().AddHours(1), "fasdf"),
                new Reserva(usuario2, usuario2, recurso2, new DateTime(), new DateTime().AddHours(1), "fasdf")
            };

            // Create ReservasRepo Mock
            reservasRepoMock = new Mock<IReservasRepo>();

            // Mock repo's methods
            reservasRepoMock.Setup(mock => mock.AsQueryable())
                .Returns(reservasAlmacenadas.AsQueryable());

            // Setup SUT
            reservasQueriesSUT = new ReservasQueriesTS(reservasRepoMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            reservasAlmacenadas = null;
            reservasRepoMock = null;
            reservasQueriesSUT = null;
        }

        [TestMethod]
        public void ReservasQueriesTS_BuscarReservas_Recurso_Valido_Salida_Correcta()
        {
            // Arrange
            var codigoRecurso = "lab";

            // Act
            var reserva = reservasQueriesSUT.BuscarReservas("", "", codigoRecurso,
                "", "").FirstOrDefault();

            // Assert
            Assert.IsNotNull(reserva);
            Assert.AreEqual(reserva.Creador.NombreUsuario, "messi");
        }

        [TestMethod]
        public void ReservasQueriesTS_BuscarReservas_Recurso_Inexistente_Salida_Vacia()
        {
            // Arrange
            var codigoRecurso = "lob";

            // Act
            var reservas = reservasQueriesSUT.BuscarReservas("", "", codigoRecurso,
                "", "");

            // Assert
            Assert.IsNotNull(reservas);
            Assert.IsTrue(reservas.Count == 0);
        }

        [TestMethod]
        public void ReservasQueriesTS_BuscarReservas_Usuario_Valido_Salida_Correcta()
        {
            // Arrange
            var nombreUsuario = "mes";

            // Act
            var reserva = reservasQueriesSUT.BuscarReservas("", "", "",
                nombreUsuario, "").FirstOrDefault();

            // Assert
            Assert.IsNotNull(reserva);
            Assert.AreEqual(reserva.Creador.NombreUsuario, "messi");
        }

        [TestMethod]
        public void ReservasQueriesTS_BuscarReservas_Usuario_Valido_Salida_Vacia()
        {
            // Arrange
            var nombreUsuario = "messoo";

            // Act
            var reservas = reservasQueriesSUT.BuscarReservas("", "", "",
                nombreUsuario, "");

            // Assert
            Assert.IsNotNull(reservas);
            Assert.IsTrue(reservas.Count == 0);
        }

        [TestMethod]
        public void ReservasQueriesTS_ReservasDelUsuario_Usuario_Valido_Salida_Correcta()
        {
            // Arrange
            var nombreUsuario = "messi";

            // Act
            var reserva = reservasQueriesSUT.ReservasDelUsuario(nombreUsuario).FirstOrDefault();

            // Assert
            Assert.IsNotNull(reserva);
            Assert.AreEqual(reserva.Responsable.NombreUsuario, nombreUsuario);
        }

        [TestMethod]
        public void ReservasQueriesTS_ReservasDelUsuario_Usuario_Valido_Salida_Vacia()
        {
            // Arrange
            var nombreUsuario = "messias";

            // Act
            var reservas = reservasQueriesSUT.ReservasDelUsuario(nombreUsuario);

            // Assert
            Assert.IsNotNull(reservas);
            Assert.IsTrue(reservas.Count == 0);
        }
    }
}
