using Dominio;
using Dominio.Repos;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using Presentacion.Soporte;

namespace Presentacion.Models
{
    public class BusquedaReservasVM : IBusquedaReservaVM
    {
        IRecursosRepo RecursosRepo;
        IUsuariosRepo UsuariosRepo;

        public IList<ReservaVM> ListaDeReservas { set; get; }

        public BusquedaReservasVM(IRecursosRepo recursosRepo, IUsuariosRepo usuariosRepo)
        {
            RecursosRepo = recursosRepo;
            UsuariosRepo = usuariosRepo;
        }

        public IEnumerable<SelectListItem> SelectEstadosDeReserva()
        {
            IList<SelectListItem> listItems = new List<SelectListItem>();
            
            listItems.Add(new SelectListItem { Text = "", Value = "", Selected = true });
            
            listItems.AddRange(typeof(EstadoReserva).ToSelectList());
            
            return listItems;
        }

        public IEnumerable<SelectListItem> SelectDeRecursos()
        {
            IList<SelectListItem> listItems = new List<SelectListItem>();
            
            listItems.Add(new SelectListItem { Text = "", Value = "", Selected = true });

            listItems.AddRange(
                RecursosRepo
                    .Todos()
                    .Select(
                        recurso =>
                            new SelectListItem
                            {
                                Text = recurso.Nombre,
                                Value = recurso.Codigo
                            }
                    )
                );

            return listItems;
        }

        public IEnumerable<SelectListItem> SelectDeUsuarios()
        {
            IList<SelectListItem> listItems = new List<SelectListItem>();

            listItems.Add(new SelectListItem { Text = "", Value = "", Selected = true });

            listItems.AddRange(
                UsuariosRepo
                    .Todos()
                    .Select(
                        usuario =>
                            new SelectListItem
                            {
                                Text = usuario.NombreUsuario,
                                Value = usuario.NombreUsuario
                            }
                    )
                );

            return listItems;
        }
    }
}