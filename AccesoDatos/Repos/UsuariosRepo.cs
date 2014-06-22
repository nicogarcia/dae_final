using Dominio;
using Dominio.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class UsuariosRepo : IUsuariosRepo
    {
        private ReservasContext reservasContext;

       
        public UsuariosRepo()
        {

        }
        public UsuariosRepo(ReservasContext reservasContext)
        {
            // TODO: Complete member initialization
            this.reservasContext = reservasContext;
        }

        public IList<Usuario> Todos ()
        {
            return reservasContext.Usuarios.ToList();
        }

        public IList<Usuario> ListarUsuarios(string key, string filtro)
        {
            if (filtro.Equals("Todos"))
            {
                return reservasContext.Usuarios.ToList();
            }
            else
            {
                return null;
            }
            
           
        }

        public void AgregarUsuario(Usuario usuario)
        {
            reservasContext.Usuarios.Add(usuario);
            reservasContext.SaveChanges();

        }

        public bool ExisteNombreUsuario(string nombreUsuario)
        {
            
            if (reservasContext.Usuarios.Where(x=>x.NombreUsuario==nombreUsuario).Count()==0)
            {
                return false;
            }
            return true;
        }

        public bool ExisteEmail(string email)
        {
            if (reservasContext.Usuarios.Where(x => x.Email == email).Count() == 0)
            {
                return false;
            }
            return true;
        }

        public bool ExisteDNI(string dni)
        {
            if (reservasContext.Usuarios.Where(x => x.DNI == dni).Count() == 0)
            {
                return false;
            }
            return true;
        }

        public bool ExisteLegajo(string legajo)
        {
            if (reservasContext.Usuarios.Where(x => x.Legajo == legajo).Count() == 0)
            {
                return false;
            }
            return true;
        }

       
    }
}
