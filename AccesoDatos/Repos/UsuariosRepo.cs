using AccesoDatos.Repos;
using Dominio;
using Dominio.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class UsuariosRepo : RepoBase<Usuario>, IUsuariosRepo
    {

        public UsuariosRepo(ReservasContext reservasContext): base(reservasContext)
        {

        }
        public Usuario getUsuario(int id = 0)
        {
            IQueryable<Usuario> consulta = Ctx.Usuarios;
            IList<Usuario> listado = consulta.Where(r => r.Id == id).ToList();
            if (listado.Count != 0)
                return listado.First();
            else
                return null;
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
            if(recursos.ToList().Count > 0)
                recursos = recursos.OrderByDescending(recurso => recurso.Nombre + recurso.Apellido);            
            return recursos.ToList();                       
        }

        public bool ExisteNombreUsuario(string nombreUsuario)
        {
            
            if (Ctx.Usuarios.Where(x=>x.NombreUsuario==nombreUsuario).Count()==0)
            {
                return false;
            }
            return true;
        }

        public bool ExisteEmail(string email)
        {
            if (Ctx.Usuarios.Where(x => x.Email == email).Count() == 0)
            {
                return false;
            }
            return true;
        }

        public bool ExisteDNI(string dni)
        {
            if (Ctx.Usuarios.Where(x => x.DNI == dni).Count() == 0)
            {
                return false;
            }
            return true;
        }

        public bool ExisteLegajo(string legajo)
        {
            if (Ctx.Usuarios.Where(x => x.Legajo == legajo).Count() == 0)
            {
                return false;
            }
            return true;
        }
        public bool ChequearExistenciaEmail(string email, int id)
        {
            return Ctx.Usuarios.Where(r => (r.Id != id & r.Email == email)).ToList().Count != 0;
        }
        public bool ChequearExistenciaDNI(string dni, int id)
        {
            return Ctx.Usuarios.Where(r => (r.Id != id & r.DNI == dni)).ToList().Count != 0;
        }
        public bool ChequearExistenciaLegajo(string legajo, int id)
        {
            return Ctx.Usuarios.Where(r => (r.Id != id & r.Legajo == legajo)).ToList().Count != 0;
        }

        public Usuario BuscarUsuario (string username)
        {
            IQueryable<Usuario> consulta = Ctx.Usuarios;
            IList<Usuario> listado = consulta.Where(r => r.NombreUsuario == username).ToList();
            if (listado.Count != 0)
                return listado.First();
            else
                return null;
        }
    }
}
