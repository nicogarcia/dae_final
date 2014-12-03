using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dominio.Entidades;
using Dominio.Queries;
using Dominio.Repos;
using Dominio.Validacion;
using Presentacion.Filters;
using Presentacion.Models;
using Dominio.UnitOfWork;
using Presentacion.Models.Conversores;
using Presentacion.Soporte;
using WebMatrix.WebData;

namespace Presentacion.Controllers
{
    public class ReservasController : Controller
    {
        // Repos
        private IReservasRepo ReservasRepo;
        private IUsuariosRepo UsuariosRepo;
        private IRecursosRepo RecursosRepo;

        // Repos queries
        private IReservasQueriesTS ReservasQueriesTS;

        // Utilidades
        private IConversorReservaMV ConversorReserva;
        private IValidadorDeReserva ValidadorReserva;

        // UoW
        private IUnitOfWorkFactory UowFactory;

        public ReservasController(
            IUnitOfWorkFactory uow,
            IReservasRepo reservasRepo,
            IRecursosRepo recursosRepo,
            IUsuariosRepo usuariosRepo,
            IReservasQueriesTS reservasQueriesTs,
            IConversorReservaMV conversorReserva,
            IValidadorDeReserva validadorDeReserva)
        {
            UowFactory = uow;
            ReservasRepo = reservasRepo;
            UsuariosRepo = usuariosRepo;
            RecursosRepo = recursosRepo;
            ReservasQueriesTS = reservasQueriesTs;
            ConversorReserva = conversorReserva;
            ValidadorReserva = validadorDeReserva;
        }

        //
        // GET: /Reservas/

        [Autorizar]
        public ActionResult Index()
        {
            var currentUsername = UsuariosRepo.BuscarUsuario(WebSecurity.CurrentUserName).NombreUsuario;

            IList<ReservaVM> misReservas = 
                ReservasQueriesTS.ReservasDelUsuario(currentUsername)
                .Select(ConversorReserva.ConvertirReserva)
                .ToList();

            return View(misReservas);
        }

        //
        // GET: /Reservas/Details/5

        [Autorizar]
        [AutorizarReserva]
        public ActionResult Details(int id = 0)
        {
            Reserva reserva = ReservasRepo.ObtenerPorId(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(ConversorReserva.ConvertirReserva(reserva));
        }

        //
        // GET: /Reservas/Create

        [Autorizar]
        public ActionResult Create()
        {
            return View(ConversorReserva.CrearReservaVM());
        }

        //
        // GET: /Reservas/CreateForResource/5

        [Autorizar]
        public ActionResult CreateForResource(int id = 0)
        {
            Recurso recurso = RecursosRepo.ObtenerPorId(id);
            if (recurso == null)
            {
                return HttpNotFound();
            }
            return View("Create", ConversorReserva.CrearReservaVM(recurso.Codigo));
        }

        //
        // POST: /Reservas/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizar]
        public ActionResult Create(ReservaVM reservaVM)
        {
            using (var uow = UowFactory.Actual)
            {
                PoblarUsuarioResponsable(reservaVM);

                if (ModelState.IsValid && ValidarReservaAuxiliar(reservaVM))
                {
                    var reserva = CrearReservaAuxiliar(reservaVM);

                    ReservasRepo.Agregar(reserva);

                    uow.Commit();

                    return RedirectToAction("Index");
                }

                ModelStateHelper.CopyErrors(ValidadorReserva.ObtenerErrores(), ModelState);

                ConversorReserva.PoblarSelectUsuario(reservaVM);

                return View(reservaVM);
            }
        }

        private void PoblarUsuarioResponsable(ReservaVM reservaVM)
        {
            string nombreResponsable = User.Identity.Name;

            if (User.IsInRole(TipoDeUsuario.Administrador.ToString()))
                nombreResponsable = reservaVM.Responsable ?? User.Identity.Name;

            reservaVM.Responsable = nombreResponsable;
        }

        private Reserva CrearReservaAuxiliar(ReservaVM reservaVM)
        {
            Usuario creador = UsuariosRepo.BuscarUsuario(User.Identity.Name);
            Usuario responsable = UsuariosRepo.BuscarUsuario(reservaVM.Responsable);
            Recurso recurso = RecursosRepo.ObtenerPorCodigo(reservaVM.RecursoReservado);

            return new Reserva(creador, responsable, recurso, reservaVM.Inicio, reservaVM.Fin, reservaVM.Descripcion);
        }

        private bool ValidarReservaAuxiliar(ReservaVM reservaVM)
        {
            return ValidadorReserva.Validar(reservaVM.Responsable, reservaVM.RecursoReservado, reservaVM.Inicio, reservaVM.Fin);
        }

        //
        // GET: /Reservas/Edit/5

        [AutorizarReserva]
        public ActionResult Edit(int id = 0)
        {
            Reserva reserva = ReservasRepo.ObtenerPorId(id);

            if (reserva == null)
            {
                return HttpNotFound();
            }

            return View(ConversorReserva.ConvertirReserva(reserva));
        }

        //
        // GET: /Reservas/EditForResource/5

        [Autorizar]
        public ActionResult EditForResource(int id = 0)
        {
            Recurso recurso = RecursosRepo.ObtenerPorId(id);
            if (recurso == null)
            {
                return HttpNotFound();
            }
            return View("Edit", ConversorReserva.CrearReservaVM(recurso.Codigo));
        }

        //
        // POST: /Reservas/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizar]
        [AutorizarReserva]
        public ActionResult Edit(ReservaVM reservaVM)
        {
            using (var uow = UowFactory.Actual)
            {
                PoblarUsuarioResponsable(reservaVM);

                if (ModelState.IsValid && ValidarReservaAuxiliar(reservaVM))
                {
                    var reserva = EditarReservaAuxiliar(reservaVM);

                    ReservasRepo.Actualizar(reserva);

                    uow.Commit();

                    return RedirectToAction("Index");
                }

                ModelStateHelper.CopyErrors(ValidadorReserva.ObtenerErrores(), ModelState);

                ConversorReserva.PoblarSelectUsuario(reservaVM);

                return View(reservaVM);
            }
        }

        private Reserva EditarReservaAuxiliar(ReservaVM reservaVM)
        {
            Reserva reserva = ReservasRepo.ObtenerPorId(reservaVM.Id);

            // Modificaciones implicitas
            reserva.Creador = UsuariosRepo.BuscarUsuario(User.Identity.Name);
            reserva.FechaCreacion = DateTime.Now;

            // Modificaciones explicitas
            reserva.RecursoReservado = RecursosRepo.ObtenerPorCodigo(reservaVM.RecursoReservado);
            reserva.Inicio = reservaVM.Inicio;
            reserva.Fin = reservaVM.Fin;
            reserva.Descripcion = reservaVM.Descripcion;

            return reserva;
        }

        //
        // GET: /Reservas/Delete/5

        [AutorizarReserva]
        public ActionResult Delete(int id = 0)
        {
            Reserva reserva = ReservasRepo.ObtenerPorId(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(ConversorReserva.ConvertirReserva(reserva));
        }

        //
        // POST: /Reservas/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Autorizar]
        [AutorizarReserva]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var uow = UowFactory.Actual)
            {
                Reserva reserva = ReservasRepo.ObtenerPorId(id);

                if (reserva == null)
                {
                    return HttpNotFound();
                }

                ReservasRepo.Eliminar(reserva);

                uow.Commit();

                return RedirectToAction("Index");
            }
        }

        [Autorizar(TipoDeUsuario.Administrador)]
        public ActionResult FullList(string fechaDesde, string fechaHasta, string codigoRecurso,
            string usuarioResponsable, string estadoReserva)
        {
            var searchVm = ConversorReserva.CrearBusquedaReservasVM();

            IList<ReservaVM> lista;

            lista = ReservasQueriesTS.BuscarReservas(
                fechaDesde,
                fechaHasta,
                codigoRecurso,
                usuarioResponsable,
                estadoReserva)
                .Select(ConversorReserva.ConvertirReserva)
                .ToList();

            searchVm.ListaDeReservas = lista;

            return View(searchVm);
        }

    }
}