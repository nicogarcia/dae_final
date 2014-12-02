using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Entidades;

namespace Dominio.Queries
{
    public interface IMultipleTypeQueriesTS
    {
        IEnumerable<Recurso> RecursosDisponibles(
            string nombre, 
            string codigo, 
            string tipo, 
            List<string> caracteristicasTipo,
            List<string> caracteristicasValor, 
            DateTime inicio, 
            DateTime fin);
    }
}
