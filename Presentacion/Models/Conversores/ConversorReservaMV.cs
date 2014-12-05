using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dominio.Entidades;
using Dominio.Repos;
using DotNetOpenAuth.Messaging;
using Presentacion.Soporte;

namespace Presentacion.Models.Conversores
{
    public class ConversorReservaMV : IConversorReservaMV
    {
        private IUsuariosRepo UsuariosRepo;
        private IRecursosRepo RecursosRepo;
        
        public ConversorReservaMV(IUsuariosRepo usuariosRepo, IRecursosRepo recursosRepo)
        {
            UsuariosRepo = usuariosRepo;
            RecursosRepo = recursosRepo;
        }

        public ReservaVM CrearReservaVM()
        {
            var reservaVM = new ReservaVM();
            
            PoblarSelectUsuario(reservaVM);
            PoblarSelectRecursos(reservaVM);

            return reservaVM;
        }

        public ReservaVM CrearReservaVM(string codigoRecurso)
        {
            var reservaVM = CrearReservaVM();

            reservaVM.RecursoReservado = codigoRecurso;

            return reservaVM;
        }

        public BusquedaReservasVM CrearBusquedaReservasVM()
        {
            var busquedaReservasVM = new BusquedaReservasVM();

            PoblarSelectUsuarios(busquedaReservasVM);
            PoblarSelectRecursos(busquedaReservasVM);
            PoblarSelectEstadosDeReserva(busquedaReservasVM);

            return busquedaReservasVM;
        }

        public ReservaVM ConvertirReserva(Reserva reserva)
        {
            var reservaVM = new ReservaVM();

            reservaVM.Id = reserva.Id;
            reservaVM.Creador = reserva.Creador.NombreUsuario;
            reservaVM.Estado = reserva.Estado.ToString();
            reservaVM.FechaCreacion = reserva.FechaCreacion;
            reservaVM.Fin = reserva.Fin;
            reservaVM.Inicio = reserva.Inicio;
            
            reservaVM.RecursoReservado = reserva.RecursoReservado.Codigo;
            reservaVM.Responsable = reserva.Responsable.NombreUsuario;
            reservaVM.Descripcion = reserva.Descripcion;

            PoblarSelectUsuario(reservaVM);

            return reservaVM;
        }

        public void PoblarSelectUsuario(ReservaVM reservaVM)
        {
            var selectList = new List<SelectListItem>();

            selectList.Add(new SelectListItem { Selected = false, Text = "", Value = "" });
            selectList.Add(new SelectListItem { Selected = true, Text = "Usuario actual", Value = "" });
            selectList.AddRange(
                UsuariosRepo.Todos()
                    .Select(
                        usuario =>
                            new SelectListItem
                            {
                                Selected = false,
                                Text = usuario.NombreUsuario,
                                Value = usuario.NombreUsuario
                            })
                );


            reservaVM.SelectUsuarioResponsable = selectList;
        }

        public void PoblarSelectRecursos(BusquedaReservasVM busquedaReservasVM)
        {
            busquedaReservasVM.SelectRecursos = SelectRecursos();
        }

        public void PoblarSelectRecursos(ReservaVM reservaVM)
        {
             reservaVM.SelectRecursos = SelectRecursos();
        }

        private IList<SelectListItem> SelectRecursos()
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

        public void PoblarSelectUsuarios(BusquedaReservasVM busquedaReservasVM)
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

            busquedaReservasVM.SelectUsuarios = listItems;
        }

        public void PoblarSelectEstadosDeReserva(BusquedaReservasVM busquedaReservasVM)
        {
            IList<SelectListItem> listItems = new List<SelectListItem>();

            listItems.Add(new SelectListItem { Text = "", Value = "", Selected = true });

            listItems.AddRange(typeof(EstadoReserva).ToSelectList());

            busquedaReservasVM.SelectEstadoReserva = listItems;
        }
    }
}