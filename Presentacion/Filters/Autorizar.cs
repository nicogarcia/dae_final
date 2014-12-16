using System.Web.Mvc;
using Dominio.Entidades;

namespace Presentacion.Filters
{
    public class Autorizar : AuthorizeAttribute
    {

        public Autorizar()
        { 
        }

        public Autorizar(TipoDeUsuario rol)
            : this()
        {
            Roles= rol.ToString();
        }
    }

}