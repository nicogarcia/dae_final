using System.Collections.Generic;

namespace Dominio.Validacion
{
    public interface IValidadorDeRecursos
    {
        IDictionary<string, string> ObtenerErrores();
        bool EsValido(Recurso recurso);
        bool EsValidoParaActualizar(Recurso recurso);
    }
}