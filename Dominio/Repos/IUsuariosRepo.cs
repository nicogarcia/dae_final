using System.Collections.Generic;
using System.Linq;
using Dominio.Entidades;

namespace Dominio.Repos
{
    public interface IUsuariosRepo
    {
        Usuario ObtenerUsuario(int id);

        Usuario BuscarUsuario(string nombreUsuario);

        bool ExisteNombreUsuario(string nombreUsuario, int id = -1);

        bool ExisteEmail(string email, int id = -1);

        bool ExisteDNI(string dni, int id = -1);

        bool ExisteLegajo(string legajo, int id = -1);
        
        IList<Usuario> Todos();

        void Agregar(Usuario usuario);

        void Actualizar(Usuario usuario);

        IQueryable<Usuario> AsQueryable();
    }
}
