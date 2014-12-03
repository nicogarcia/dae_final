using System;
using System.Collections.Generic;

namespace Dominio.Validacion
{
    public interface IValidadorDeReserva
    {
        IDictionary<string, string> ObtenerErrores();
        bool Validar(string usuarioResponsable, string codigoRecurso, DateTime inicio, DateTime fin);
    }
}