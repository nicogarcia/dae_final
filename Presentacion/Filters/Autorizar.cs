using System.Web;
using Dominio;
using System.Web.Mvc;
using Dominio.Repos;
using WebMatrix.WebData;

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
            this.Roles= rol.ToString();
        }
    }

}