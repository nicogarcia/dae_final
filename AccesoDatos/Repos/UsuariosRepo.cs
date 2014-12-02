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
        public Usuario getUsuario(int id = 0)
        {
            return Ctx.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
        }

        public bool ExisteNombreUsuario(string nombreUsuario)
        {
            return Ctx.Usuarios.Any(usuario => usuario.NombreUsuario == nombreUsuario);
        }

        public bool ExisteEmail(string email)
        {
            return Ctx.Usuarios.Any(usuario => usuario.Email == email);
        }

        public bool ExisteDNI(string dni)
        {
            return Ctx.Usuarios.Any(usuario => usuario.DNI == dni);
        }

        public bool ExisteLegajo(string legajo)
        {
            return Ctx.Usuarios.Any(usuario => usuario.Legajo == legajo);
        }

        public bool ChequearExistenciaEmail(string email, int id)
        {
            return Ctx.Usuarios.Any(usuario => (usuario.Id != id & usuario.Email == email));
        }

        public bool ChequearExistenciaDNI(string dni, int id)
        {
            return Ctx.Usuarios.Any(usuario => (usuario.Id != id & usuario.DNI == dni));
        }

        public bool ChequearExistenciaLegajo(string legajo, int id)
        {
            return Ctx.Usuarios.Any(usuario => (usuario.Id != id & usuario.Legajo == legajo));
        }

        public Usuario BuscarUsuario (string username)
        {
            return Ctx.Usuarios.FirstOrDefault(usuario => usuario.NombreUsuario == username);
        }
    }
}
