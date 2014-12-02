using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dominio;
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

        // UoW
        private IUnitOfWorkFactory UowFactory;

        public ReservasController(
            IUnitOfWorkFactory uow, 
            IReservasRepo reservasRepo,
            IRecursosRepo recursosRepo,
            IUsuariosRepo usuariosRepo,
            IReservasQueriesTS reservasQueriesTs)
        {
            UowFactory = uow;
            ReservasRepo = reservasRepo;
            UsuariosRepo = usuariosRepo;
            RecursosRepo = recursosRepo;
            ReservasQueriesTS = reservasQueriesTs;
        }

        //
        // GET: /Reservas/

        [Autorizar]
        public ActionResult Index()
        {
            var currentUser = UsuariosRepo.BuscarUsuario(WebSecurity.CurrentUserName);
            
            // TODO: FIX, INEFFICIENT!
            IList<ReservaVM> lista = ReservasRepo.Todos()
                .Where(reserva => reserva.Creador == currentUser)
                .Select(ConversorReservaMV.convertirReserva)
                .ToList();

            return View(lista);
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
            return View(ConversorReservaMV.convertirReserva(reserva));
        }

        //
        // GET: /Reservas/Create

        [Autorizar]
        public ActionResult Create()
        {
            return View(new ReservaVM());
        }

        //
        // GET: /Reservas/Create/5

        [Autorizar]
        public ActionResult CreateForResource(int id = 0)
        {
            Recurso recurso = RecursosRepo.ObtenerPorId(id);
            if (recurso == null)
            {
                return HttpNotFound();
            }
            return View("Create", new ReservaVM(recurso.Codigo));
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
                var validador = new ValidadorDeReserva(ReservasRepo, UsuariosRepo, RecursosRepo);

                if (ModelState.IsValid && CrearReservaAuxiliar(reservaVM, validador))
                {
                    uow.Commit();

                    return RedirectToAction("Index");
                }

                ModelStateHelper.CopyErrors(validador.Errores, ModelState);
                return View(reservaVM);
            }
        }

        private bool CrearReservaAuxiliar(ReservaVM reservaVM, ValidadorDeReserva validador)
        {
            string nombreResponsable = User.Identity.Name;

            if (User.IsInRole(TipoDeUsuario.Administrador.ToString()))
            {
                nombreResponsable = reservaVM.Responsable ?? User.Identity.Name;
            }

            if (validador.Validar(nombreResponsable, reservaVM.RecursoReservado, reservaVM.Inicio, reservaVM.Fin))
            {
                Usuario creador = UsuariosRepo.BuscarUsuario(User.Identity.Name);
                Usuario responsable = UsuariosRepo.BuscarUsuario(nombreResponsable);
                Recurso recurso = RecursosRepo.ObtenerPorCodigo(reservaVM.RecursoReservado);
                
                var reserva = new Reserva(creador, responsable, recurso, reservaVM.Inicio, reservaVM.Fin, reservaVM.Descripcion);

                ReservasRepo.Agregar(reserva);

                return true;
            }

            return false;
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

            return View(ConversorReservaMV.convertirReserva(reserva));
        }

        //
        // POST: /Reservas/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizar]
        public ActionResult Edit(ReservaVM reserva)
        {
            using (var uow = UowFactory.Actual)
            {
                if (ModelState.IsValid)
                {
                    ReservasRepo.Actualizar(ConversorReservaMV.convertirReserva(reserva));
                    
                    uow.Commit();

                    return RedirectToAction("Index");
                }
                return View(reserva);
            }
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
            return View(ConversorReservaMV.convertirReserva(reserva));
        }

        //
        // POST: /Reservas/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Autorizar]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var uow = UowFactory.Actual)
            {
                Reserva reserva = ReservasRepo.ObtenerPorId(id);

                ReservasRepo.Eliminar(reserva);

                uow.Commit();

                return RedirectToAction("Index");
            }
        }

        [Autorizar(TipoDeUsuario.Administrador)]
        public ActionResult FullList(string fechaDesde, string fechaHasta, string codigoRecurso,
            string usuarioResponsable, string estadoReserva)
        {
            // TODO: REMOVE DEPENDENCIES
            var searchVm = new BusquedaReservasVM(RecursosRepo, UsuariosRepo);

            IList<ReservaVM> lista = new List<ReservaVM>();

            foreach (Reserva x in ReservasQueriesTS.BuscarReservas(fechaDesde, fechaHasta,
                codigoRecurso, usuarioResponsable, estadoReserva))
            {
                lista.Add(ConversorReservaMV.convertirReserva(x));
            }

            searchVm.ListaDeReservas = lista;

            return View(searchVm);
        }

    }
}