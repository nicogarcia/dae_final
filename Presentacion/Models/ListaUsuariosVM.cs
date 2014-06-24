using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public bool Bloqueado (int id)
        {
            Usuario usuario = (Usuario) ListaUsuario.Where(x=> x.Id == id);
            if (usuario.EstadoUsuario == EstadoUsuario.Bloqueado)
            {
                return true;
            }
            
            return false;
        }

        public bool Activo (int id)
        {
            Usuario usuario = (Usuario) ListaUsuario.Where(x=> x.Id == id);
            if (usuario.EstadoUsuario == EstadoUsuario.Activo)
            {
                return true;
            }
            
            return false;
        }

        public string ObtenerAccionBloquear(int usuarioId)
        {
            Usuario usuario = (Usuario)ListaUsuario.Where(x => x.Id == usuarioId).FirstOrDefault();
            string accion = string.Empty;
            if (usuario != null)
            {
                if (usuario.EstadoUsuario == EstadoUsuario.Activo)
                    accion = "Lock";
                if (usuario.EstadoUsuario == EstadoUsuario.Bloqueado)
                    accion = "Unlock";
            }
            return accion;
        }

        public ListaUsuariosVM(IEnumerable<Usuario> listausario)
        {
            ListaUsuario = listausario;
        }

        public ListaUsuariosVM()
        {
            ListaUsuario = new List<Usuario>();
        }
      

    }
}
