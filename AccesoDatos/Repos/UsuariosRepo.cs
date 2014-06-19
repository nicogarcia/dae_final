using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class UsuariosRepo
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
            IList<Usuario> listadoUsuariosActivos = new List<Usuario>();
            IList<Usuario> listadoUsuarios = reservasContext.Usuarios.ToList();
            foreach(Usuario usuario in listadoUsuarios)
                if(!usuario.EstadoUsuario.Nombre.Equals("Inactivo"))
                    listadoUsuariosActivos.Add(usuario);
            return listadoUsuariosActivos;
        }

        public void AgregarUsuario(Usuario usuario)
        {
            reservasContext.Usuarios.Add(usuario);
            reservasContext.SaveChanges();
        }
    }
}
