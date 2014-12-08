using System.Collections.Generic;
using System.Linq;
using Dominio;
using Dominio.Entidades;
using Dominio.Repos;

namespace AccesoDatos.Repos
{
    public class UsuariosRepo : RepoBase<Usuario>, IUsuariosRepo
    {
        public UsuariosRepo(ReservasContext reservasContext): base(reservasContext)
        {
        }

        public Usuario ObtenerUsuario(int id = 0)
        {
            return Ctx.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
        }

        public bool ExisteNombreUsuario(string nombreUsuario, int id = -1)
        {
            return Ctx.Usuarios.Any(usuario => usuario.Id != id && usuario.NombreUsuario == nombreUsuario);
        }

        public bool ExisteEmail(string email, int id = -1)
        {
            return Ctx.Usuarios.Any(usuario => (usuario.Id != id && usuario.Email == email));
        }

        public bool ExisteDNI(string dni, int id = -1)
        {
            return Ctx.Usuarios.Any(usuario => (usuario.Id != id && usuario.DNI == dni));
        }

        public bool ExisteLegajo(string legajo, int id = -1)
        {
            return Ctx.Usuarios.Any(usuario => (usuario.Id != id && usuario.Legajo == legajo));
        }

        public Usuario BuscarUsuario (string username)
        {
            return Ctx.Usuarios.FirstOrDefault(usuario => usuario.NombreUsuario == username);
        }
    }
}
