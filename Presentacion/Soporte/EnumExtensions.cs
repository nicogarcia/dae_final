using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Presentacion.Soporte
{
    /// <summary>
    /// Métodos de extensión sobre lo enumerados
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Convierte los valores de un enumerado en una lista de SelectListItems, 
        /// donde el Value es el indice numérico y el Text es el nombre del valor del enumerado.
        /// </summary>
        /// <param name="tipoEnum">Tipo del enumerado</param>
        /// <returns>Lista de SelectListItems utilizada en las DropDownLists</returns>
        public static IEnumerable<SelectListItem> ToSelectList(this Type tipoEnum)
        {
            return from Enum e in Enum.GetValues(tipoEnum)
                            select new SelectListItem() { Value = e.GetHashCode().ToString(), 
                                                          Text = e.ToString() };
        }

    }
}