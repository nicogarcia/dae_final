using Dominio;
using System.Web.Mvc;

namespace Presentacion.Filters
{
    public class Autorizar : AuthorizeAttribute
    {

        public Autorizar() : base()
        { 
        }

        public Autorizar(TipoDeUsuario rol)
            : this()
        {
            Roles= rol.ToString();
        }
    }

}