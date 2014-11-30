using System.Collections.Generic;
using System.Web.Mvc;

namespace Presentacion.Soporte
{
    public class ModelStateHelper
    {
          public static void CopyErrors(IDictionary<string, string> errores, ModelStateDictionary ModelState)
            {
                if (errores != null)
                {
                    ICollection<string> keys = errores.Keys;
                    string mensajedeerror;
                    foreach (string key in keys)
                    {
                        errores.TryGetValue(key, out mensajedeerror);
                        ModelState.AddModelError(key, mensajedeerror);
                    }
                }
            }
    }
}