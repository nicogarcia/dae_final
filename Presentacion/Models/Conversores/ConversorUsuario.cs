using System.Web.Security;
using Dominio;
using Dominio.Entidades;
using Dominio.Queries;
using Dominio.Repos;
using WebMatrix.WebData;

namespace Presentacion.Models.Conversores
{
    public class ConversorUsuario : IConversorUsuario
    {
        private IUsuariosRepo UsuariosRepo;

        public ConversorUsuario(IUsuariosRepo usuariosRepo)
        {
            UsuariosRepo = usuariosRepo;
        }

        public UsuarioVM CrearViewModel(Usuario u)
        {
            var usuarioVM = new UsuarioVM(u.Id);

            usuarioVM.Apellido = u.Apellido;
            usuarioVM.DNI = u.DNI;
            usuarioVM.Email = u.Email;
            usuarioVM.EstadoUsuario = u.EstadoUsuario.ToString();
            usuarioVM.Legajo = u.Legajo;
            usuarioVM.Nombre = u.Nombre;
            usuarioVM.NombreUsuario = u.NombreUsuario;
            usuarioVM.Telefono = u.Telefono;
            usuarioVM.Tipo = u.Tipo;

            return usuarioVM;
        }

        public Usuario CrearUsuario(UsuarioVM usuarioVM)
        {
            var usuario = new Usuario(
                usuarioVM.NombreUsuario,
                usuarioVM.Nombre,
                usuarioVM.Apellido,
                usuarioVM.DNI,
                usuarioVM.Legajo,
                usuarioVM.Email,
                usuarioVM.Telefono,
                usuarioVM.Tipo);

            WebSecurity.CreateUserAndAccount(usuarioVM.NombreUsuario, usuarioVM.Password);

            // Le asocio el rol correspondiente.
            var roles = (SimpleRoleProvider)Roles.Provider;
            roles.AddUsersToRoles(new[] { usuarioVM.NombreUsuario }, new[] { usuarioVM.Tipo.ToString() });

            return usuario;
        }
        
        public Usuario ActualizarUsuario(UsuarioVM usuarioVM)
        {
            var usuario = UsuariosRepo.ObtenerUsuario(usuarioVM.id);

            usuario.Apellido = usuarioVM.Apellido;
            usuario.DNI = usuarioVM.DNI;
            usuario.Email = usuarioVM.Email;
            usuario.Legajo = usuarioVM.Legajo;
            usuario.Nombre = usuarioVM.Nombre;
            usuario.Telefono = usuarioVM.Telefono;
            usuario.Tipo = usuarioVM.Tipo;

            var roles = (SimpleRoleProvider) Roles.Provider;
            if (usuarioVM.Tipo != usuario.Tipo)
            {
                roles.RemoveUsersFromRoles(new[] {usuario.NombreUsuario}, new[] {usuario.Tipo.ToString()});
                roles.AddUsersToRoles(new[] {usuario.NombreUsuario}, new[] {usuarioVM.Tipo.ToString()});
            }

            return usuario;
        }
    }
}