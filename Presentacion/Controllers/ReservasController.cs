using System.Collections.Generic;
using System.Web.Mvc;
using Dominio;
using Dominio.Repos;
using Presentacion.Filters;
using Presentacion.Models;
using Dominio.UnitOfWork;
using Presentacion.Models.Conversores;

namespace Presentacion.Controllers
{
    public class ReservasController : Controller
    {
        public IUnitOfWorkFactory Uow;
        public IReservaRepo ReservasRepo;

        public ReservasController(IUnitOfWorkFactory uow, IReservaRepo reservasRepo)
        {
            Uow = uow;
            ReservasRepo = reservasRepo;
        }

        //
        // GET: /Reservas/

        [Autorizar(TipoDeUsuario.Miembro)]
        public ActionResult Index()
        {
            IList<ReservaVM> lista = new List<ReservaVM>();

            foreach (Reserva x in ReservasRepo.Todos()){
                lista.Add(ConversorReservaMV.convertirReserva(x));
            }

            return View(lista);
        }

        //
        // GET: /Reservas/Details/5

        [Autorizar(TipoDeUsuario.Miembro)]
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

        [Autorizar(TipoDeUsuario.Miembro)]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Reservas/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizar(TipoDeUsuario.Miembro)]
        public ActionResult Create(ReservaVM reserva)
        {
            using (var uow = Uow.Actual)
            {
                if (ModelState.IsValid)
                {
                    ReservasRepo.Agregar(ConversorReservaMV.convertirReserva(reserva));

                    uow.Commit();

                    return RedirectToAction("Index");
                }

                return View(reserva);
            }
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
        [Autorizar(TipoDeUsuario.Miembro)]
        public ActionResult Edit(ReservaVM reserva)
        {
            using (var uow = Uow.Actual)
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
        [Autorizar(TipoDeUsuario.Miembro)]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var uow = Uow.Actual)
            {
                Reserva reserva = ReservasRepo.ObtenerPorId(id);

                ReservasRepo.Eliminar(reserva);

                uow.Commit();

                return RedirectToAction("Index");
            }
        }

        [Autorizar(TipoDeUsuario.Administrador)]
        public ActionResult ListAndSearch(string fecha_desde, string fecha_hasta, string tipo_recurso, string usuario_responsable, string estado_reserva)
        {
            ReservasSearchContainer toR = new ReservasSearchContainer();

            IList<ReservaVM> lista = new List<ReservaVM>();

            foreach (Reserva x in ReservasRepo.buscarReservas(fecha_desde, fecha_hasta, 
                tipo_recurso, usuario_responsable, estado_reserva))
            {
                lista.Add(Models.Conversores.ConversorReservaMV.convertirReserva(x));
            }

            toR.list = lista;

            return View(toR);
        }

    }
}