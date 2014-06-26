using Dominio;
using System.Web.Mvc;

namespace Presentacion.Filters
{
    public class Autorizar : AuthorizeAttribute
    {

        public Autorizar() : base()
        { 
        }

        public Autorizar(TipoDeUsuario rol, EstadoUsuario estado)
            : this()
        {
            this.Roles= rol.ToString()+","+estado.ToString();
        }
    }
}