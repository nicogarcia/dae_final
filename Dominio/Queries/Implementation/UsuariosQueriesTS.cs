using System.Collections.Generic;
using System.Linq;
using Dominio.Entidades;
using Dominio.Repos;

namespace Dominio.Queries.Implementation
{
    public class UsuariosQueriesTS : IUsuariosQueriesTS
    {
        IUsuariosRepo UsuariosRepo;

        public UsuariosQueriesTS(IUsuariosRepo usuariosRepo)
        {
            UsuariosRepo = usuariosRepo;
        }

        public IList<Usuario> ListarUsuarios(string filtronombre, string filtroapellido, string filtrolegajo)
        {
            // Query de recursos
            IQueryable<Usuario> recursos = UsuariosRepo.AsQueryable();

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

    }
}
