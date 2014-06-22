using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Repos
{
    public interface IUsuariosRepo
    {
        bool ExisteNombreUsuario(string nombreUsuario);

        bool ExisteEmail(string email);

        bool ExisteDNI(string dni);

        bool ExisteLegajo(string legajo);

        IList<Usuario> ListarUsuarios(string key, string filtro);

        void AgregarUsuario(Usuario usuario);
    }
}
