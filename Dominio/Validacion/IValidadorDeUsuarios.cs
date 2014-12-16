using System.Collections.Generic;

namespace Dominio.Validacion
{
    public interface IValidadorDeUsuarios
    {
        IDictionary<string, string> ObtenerErrores();
        bool Validar(string username, string email, string dni, string legajo, int usuarioId = -1);
    }
}