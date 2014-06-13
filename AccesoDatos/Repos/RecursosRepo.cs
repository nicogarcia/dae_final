using System.Collections.Generic;
using System.Linq;
using Dominio;

namespace AccesoDatos.Repos
{
    public class RecursosRepo : RepoBase<Recurso>
    {
        public RecursosRepo(ReservasContext ctx) : base(ctx)
        {
        }

        public IList<Recurso> FiltrarYOrdenar(string orden, string filtroCodigo, string filtroTipo, string filtroNombre)
        {
            // Query de recursos
            IQueryable<Recurso> recursos = Ctx.Recursos;

            // No mostrar los recursos inactivos
            recursos = recursos.Where(recurso => recurso.EstadoActual == EstadoRecurso.Activo);

            // Aplicar filtros
            if (!string.IsNullOrEmpty(filtroCodigo))
                recursos = recursos.Where(r => r.Codigo.ToUpper().Contains(filtroCodigo.ToUpper()));

            if (!string.IsNullOrEmpty(filtroNombre))
                recursos = recursos.Where(r => r.Nombre.ToUpper().Contains(filtroNombre.ToUpper()));

            if (!string.IsNullOrEmpty(filtroTipo))
            {
                int filtroTipoInt = int.Parse(filtroTipo);
                recursos = recursos.Where(r => r.Tipo.Id == filtroTipoInt);
            }

            // Ordenar
            switch (orden)
            {
                case "nombre_desc":
                    recursos = recursos.OrderByDescending(recurso => recurso.Nombre);
                    break;
                case "codigo":
                    recursos = recursos.OrderBy(recurso => recurso.Codigo);
                    break;
                case "codigo_desc":
                    recursos = recursos.OrderByDescending(recurso => recurso.Codigo);
                    break;
                case "tipo":
                    recursos = recursos.OrderBy(recurso => recurso.Tipo.Nombre);
                    break;
                case "tipo_desc":
                    recursos = recursos.OrderByDescending(recurso => recurso.Tipo.Nombre);
                    break;
                default:
                    recursos = recursos.OrderBy(recurso => recurso.Nombre);
                    break;
            }

            return recursos.ToList();
        }

    }
}