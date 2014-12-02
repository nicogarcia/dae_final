using System.Collections.Generic;
using Dominio.Entidades;

namespace Dominio.Validacion
{
    public interface IValidadorDeRecursos
    {
        IDictionary<string, string> ObtenerErrores();
        bool EsValido(Recurso recurso);
        bool EsValidoParaActualizar(Recurso recurso);
    }
}