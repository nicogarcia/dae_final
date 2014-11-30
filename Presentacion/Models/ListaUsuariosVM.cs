using Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Presentacion.Models
{
    public class ListaUsuariosVM : Controller
    {
        //
        // GET: /ListaUsuariosVM/
        public IEnumerable<Usuario> ListaUsuario { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Legajo { get; set; }
        
        public ListaUsuariosVM(IEnumerable<Usuario> listausario)
        {
            ListaUsuario = listausario;
        }

        public ListaUsuariosVM()
        {
            ListaUsuario = new List<Usuario>();
        }

        public bool Bloqueado(int id)
        {
            var usuario = ListaUsuario.First(x => x.Id == id);

            return usuario.EstadoUsuario == EstadoUsuario.Bloqueado;
        }

        public bool Activo(int id)
        {
            var usuario = ListaUsuario.First(x => x.Id == id);

            return usuario.EstadoUsuario == EstadoUsuario.Activo;
        }
    }
}
