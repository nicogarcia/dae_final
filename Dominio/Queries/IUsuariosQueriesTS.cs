using System.Collections.Generic;
using Dominio.Entidades;

namespace Dominio.Queries
{
    public interface IUsuariosQueriesTS
    {
        IList<Usuario> ListarUsuarios(string filtronombre, string filtroapellido, string filtrolegajo);
    }
}
