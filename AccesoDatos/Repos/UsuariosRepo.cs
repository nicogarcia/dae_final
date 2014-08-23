using System.Collections.Generic;
using System.Linq;
using Dominio;
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

        public IList<Usuario> ListarUsuarios(string filtronombre, string filtroapellido, string filtrolegajo)
        {
            // Query de recursos
            IQueryable<Usuario> recursos = Ctx.Usuarios;

            // Aplicar filtros
            if (!string.IsNullOrEmpty(filtronombre))
                recursos = recursos.Where(r => r.Nombre.ToUpper().Contains(filtronombre.ToUpper()));

            if (!string.IsNullOrEmpty(filtroapellido))
                recursos = recursos.Where(r => r.Apellido.ToUpper().Contains(filtroapellido.ToUpper()));

            if (!string.IsNullOrEmpty(filtrolegajo))
            {
                recursos = recursos.Where(r => r.Legajo.ToUpper().Contains(filtrolegajo.ToUpper()));
            }

            recursos = recursos.OrderByDescending(recurso => recurso.Nombre + recurso.Apellido);   
         
            return recursos.ToList();                       
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
