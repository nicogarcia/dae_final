using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AccesoDatos;
using AccesoDatos.Repos;
using Dominio.Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ReservasDCICTest.AccesoDatos
{
    [TestClass]
    public class UsuariosRepoTest
    {
        private UsuariosRepo usuariosRepoSUT;
        private Mock<IReservasContext> dbContextMock;

        private IList<Usuario> usuariosAlmacenados;

        [TestInitialize]
        public void Setup()
        {
            usuariosAlmacenados = new List<Usuario>
            {
                new Usuario("messi", "Lionel", "Messi", "23424", "4365",
                    "messi@messi.com", "434545", TipoDeUsuario.Administrador)
            };

            var data = usuariosAlmacenados.AsQueryable();

            // Mock the DbSet
            var usuariosDbSetMock = new Mock<IDbSet<Usuario>>();
            usuariosDbSetMock.Setup(mock => mock.Provider).Returns(data.Provider);
            usuariosDbSetMock.Setup(mock => mock.Expression).Returns(data.Expression);
            usuariosDbSetMock.Setup(mock => mock.ElementType).Returns(data.ElementType);
            usuariosDbSetMock.Setup(mock => mock.GetEnumerator()).Returns(data.GetEnumerator());

            // Mock the Context
            dbContextMock = new Mock<IReservasContext>();

            // Set the context's DbSet to the mocked one
            dbContextMock.Setup(mock => mock.Usuarios).Returns(usuariosDbSetMock.Object);

            usuariosRepoSUT = new UsuariosRepo(dbContextMock.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {
            dbContextMock = null;
            usuariosRepoSUT = null;
            usuariosAlmacenados = null;
        }

        [TestMethod]
        public void UsuariosRepo_ExisteMail_Existente_Retorna_Verdadero()
        {
            // Arrange
            string email = "messi@messi.com";

            // Act
            bool result = usuariosRepoSUT.ExisteEmail(email);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UsuariosRepo_ExisteMail_Inexistente_Retorna_Falso()
        {
            // Arrange
            string email = "lionel@messi.com";

            // Act
            bool result = usuariosRepoSUT.ExisteEmail(email);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UsuariosRepo_BuscarUsuario_Existente_Retorna_Usuario_Correcto()
        {
            // Arrange
            var nombreUsuario = "messi";
            var usuarioEsperado = usuariosAlmacenados.First(u => u.NombreUsuario == nombreUsuario);
            
            // Act
            var usuario = usuariosRepoSUT.BuscarUsuario(nombreUsuario);
            
            // Assert
            Assert.AreSame(usuario, usuarioEsperado);
        }

        [TestMethod]
        public void UsuariosRepo_BuscarUsuario_Inexistente_Retorna_Null()
        {
            // Arrange
            var nombreUsuario = "ronaldo";

            // Act
            var usuario = usuariosRepoSUT.BuscarUsuario(nombreUsuario);

            // Assert
            Assert.IsNull(usuario);
        }

        [TestMethod]
        public void UsuariosRepo_ObtenerUsuario_Existente_Retorna_Usuario_Correcto()
        {
            // Arrange
            var id = 0;
            var usuarioEsperado = usuariosAlmacenados.First(u => u.Id == id);

            // Act
            var usuario = usuariosRepoSUT.ObtenerUsuario(id);

            // Assert
            Assert.AreSame(usuario, usuarioEsperado);
        }

        [TestMethod]
        public void UsuariosRepo_ObtenerUsuario_Inexistente_Retorna_Null()
        {
            // Arrange
            var id = 10;

            // Act
            var usuario = usuariosRepoSUT.ObtenerUsuario(id);

            // Assert
            Assert.IsNull(usuario);
        }

    }
}
