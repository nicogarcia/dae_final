﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dominio;
using Dominio.Repos;
using Presentacion.Filters;
using Presentacion.Models;
using Dominio.UnitOfWork;
using Presentacion.Models.Conversores;
using Presentacion.Soporte;

namespace Presentacion.Controllers
{
    public class ReservasController : Controller
    {
        IUnitOfWorkFactory Uow;
        IReservaRepo ReservasRepo;
        IUsuariosRepo UsuariosRepo;
        IRecursosRepo RecursosRepo;

        public ReservasController(IUnitOfWorkFactory uow, IReservaRepo reservasRepo,
            IRecursosRepo recursosRepo, IUsuariosRepo usuariosRepo)
        {
            Uow = uow;
            ReservasRepo = reservasRepo;
            UsuariosRepo = usuariosRepo;
            RecursosRepo = recursosRepo;
        }

        //
        // GET: /Reservas/

        [Autorizar()]
        public ActionResult Index()
        {
            IList<ReservaVM> lista = ReservasRepo.Todos()
                .Select(reservaVM => ConversorReservaMV.convertirReserva(reservaVM))
                .ToList();

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

        [Autorizar()]
        public ActionResult Create()
        {
            return View(new ReservacreateVM(User.IsInRole(TipoDeUsuario.Administrador.ToString())));
        }

        //
        // POST: /Reservas/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizar()]
        public ActionResult Create(ReservacreateVM reservaVM)
        {
            using (var uow = Uow.Actual)
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

        private bool CrearReservaAuxiliar(ReservacreateVM reservaVM, ValidadorDeReserva validador)
        {
            string responsable = User.Identity.Name;

            if (User.IsInRole(TipoDeUsuario.Administrador.ToString()))
            {
                responsable = reservaVM.Responsable ?? User.Identity.Name;
            }

            if (validador.Validar(responsable, reservaVM.RecursoReservado, reservaVM.Inicio, reservaVM.Fin))
            {
                Recurso recurso = RecursosRepo.ObtenerPorCodigo(reservaVM.RecursoReservado);

                Reserva reserva = ReservasRepo.CrearReserva(User.Identity.Name, responsable, recurso, reservaVM.Inicio,
                    reservaVM.Fin, reservaVM.Descripcion);

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
                lista.Add(ConversorReservaMV.convertirReserva(x));
            }

            toR.list = lista;

            return View(toR);
        }

    }
}