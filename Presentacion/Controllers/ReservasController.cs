using System.Web.Mvc;
using Dominio;
using Dominio.Autorizacion;
using Dominio.Repos;
using Presentacion.Models;
using Dominio.UnitOfWork;
using WebMatrix.WebData;
using System.Collections.Generic;
namespace Presentacion.Controllers
{
    public class ReservasController : Controller
    {
        public IUnitOfWorkFactory Uow;
        public IReservaRepo ReservasRepo;
        public IAutorizacionUsuario AutorizacionUsuario;

        public ReservasController(IUnitOfWorkFactory uow, IReservaRepo reservasRepo, 
            IAutorizacionUsuario autorizacionUsuario)
        {
            Uow = uow;
            ReservasRepo = reservasRepo;
            AutorizacionUsuario = autorizacionUsuario;
        }

        //
        // GET: /Reservas/

        public ActionResult Index()
        {
            IList<Presentacion.Models.ReservaVM> lista = new List<Presentacion.Models.ReservaVM>();
            foreach (Reserva x in ReservasRepo.Todos()){
                lista.Add(Presentacion.Models.Conversores.ConversorReservaMV.convertirReserva(x));
            }
            return View(lista);
        }

        //
        // GET: /Reservas/Details/5

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

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Reservas/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        public ActionResult Edit(int id = 0)
        {
            Reserva reserva = ReservasRepo.ObtenerPorId(id);

            if (reserva == null)
            {
                return HttpNotFound();
            }

            if(!AutorizacionUsuario.AutorizarSobreReserva(WebSecurity.CurrentUserId, reserva))
                return new HttpUnauthorizedResult();

            return View(reserva);
        }

        //
        // POST: /Reservas/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        public ActionResult ListAndSearch(string fecha_desde, string fecha_hasta, string tipo_recurso, string usuario_responsable, string estado_reserva)
        {
            using (var uow = Uow.Actual)
            {
                
                uow.Commit();

                return RedirectToAction("Index");
            }
        }

    }
}