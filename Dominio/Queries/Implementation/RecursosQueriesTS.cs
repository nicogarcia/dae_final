using System.Collections.Generic;
using System.Linq;
using Dominio.Entidades;
using Dominio.Repos;

namespace Dominio.Queries.Implementation
{
    public class RecursosQueriesTS : IRecursosQueriesTS
    {
        IRecursosRepo RecursosRepo;

        public RecursosQueriesTS(IRecursosRepo recursos)
        {
            RecursosRepo = recursos;
        }

        public IList<Recurso> FiltrarYOrdenar(string orden, string filtroCodigo, string filtroTipo, string filtroNombre)
        {
            // Query de recursos
            IQueryable<Recurso> recursos = Filtrar(filtroCodigo, filtroNombre, filtroTipo);

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


        public IQueryable<Recurso> Filtrar(string filtroCodigo, string filtroNombre, string filtroTipo)
        {
            // Query de recursos
            IQueryable<Recurso> recursos = RecursosRepo.AsQueryable();

            // No mostrar los recursos inactivos
            recursos = recursos.Where(recurso => recurso.EstadoActual == EstadoRecurso.Activo);

            // Aplicar filtros
            if (!string.IsNullOrEmpty(filtroCodigo))
                recursos = recursos.Where(recurso => recurso.Codigo.ToUpper().Contains(filtroCodigo.ToUpper()));

            if (!string.IsNullOrEmpty(filtroNombre))
                recursos = recursos.Where(recurso => recurso.Nombre.ToUpper().Contains(filtroNombre.ToUpper()));

            if (!string.IsNullOrEmpty(filtroTipo))
            {
                int filtroTipoInt = int.Parse(filtroTipo);
                recursos = recursos.Where(recurso => recurso.Tipo.Id == filtroTipoInt);
            }

            return recursos;
        }
    }
}
