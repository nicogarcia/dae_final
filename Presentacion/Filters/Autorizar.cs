using Dominio;
using System.Web.Mvc;
using Dominio.Entidades;

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