using System.Web.Mvc;
using Dominio;
using Dominio.Repos;
using Dominio.UnitOfWork;
using Presentacion.Filters;

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
            return View(ReservasRepo.Todos());
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
            return View(reserva);
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
        public ActionResult Create(Reserva reserva)
        {
            using (var uow = Uow.Actual)
            {
                if (ModelState.IsValid)
                {
                    ReservasRepo.Agregar(reserva);

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

            return View(reserva);
        }

        //
        // POST: /Reservas/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizar(TipoDeUsuario.Miembro)]
        public ActionResult Edit(Reserva reserva)
        {
            using (var uow = Uow.Actual)
            {
                if (ModelState.IsValid)
                {
                    ReservasRepo.Actualizar(reserva);
                    
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
            return View(reserva);
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
    }
}