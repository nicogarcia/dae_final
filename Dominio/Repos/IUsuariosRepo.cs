using System.Collections.Generic;
using System.Linq;
using Dominio.Entidades;

namespace Dominio.Repos
{
    public interface IUsuariosRepo
    {
        bool ExisteNombreUsuario(string nombreUsuario);

        Usuario getUsuario(int id);

        bool ExisteEmail(string email);

        bool ExisteDNI(string dni);

        bool ExisteLegajo(string legajo);
        
        IList<Usuario> Todos();

        void Agregar(Usuario usuario);

        void Actualizar(Usuario usuario);
        
        bool ChequearExistenciaEmail(string email, int id);

        bool ChequearExistenciaDNI(string dni, int id);

        bool ChequearExistenciaLegajo(string legajo, int id);

        Usuario BuscarUsuario(string usuarioResponsable);

        IQueryable<Usuario> AsQueryable();
    }
}
