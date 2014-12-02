using System.Collections.Generic;
using System.Linq;
using Dominio.Entidades;

namespace Dominio.Queries
{
    public interface IRecursosQueriesTS
    {
        IList<Recurso> FiltrarYOrdenar(string orden, string filtroCodigo, string filtroTipo, string filtroNombre);
        
        IQueryable<Recurso> Filtrar(string filtroCodigo, string filtroNombre, string filtroTipo);
    }
}
