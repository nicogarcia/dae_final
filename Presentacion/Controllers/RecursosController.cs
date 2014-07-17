﻿using Dominio;
using Dominio.Repos;
using Dominio.UnitOfWork;
using Presentacion.Filters;
using Presentacion.Models;
using System.Web.Mvc;
using Presentacion.Soporte;

namespace Presentacion.Controllers
{
    public class RecursosController : Controller
    {
        private IRecursosRepo RecursosRepo;
        private ITiposDeRecursosRepo TiposDeRecursosRepo;
        private ITiposDeCaracteristicasRepo TiposDeCaracteristicasRepo;
        private IUnitOfWorkFactory uowFactory;

        public RecursosController(IRecursosRepo recursosRepo, ITiposDeCaracteristicasRepo tiposDeCaracteriscasRepo,
            ITiposDeRecursosRepo tiposDeRecursosRepo, IUnitOfWorkFactory uowFactory)
        {
            RecursosRepo = recursosRepo;
            TiposDeRecursosRepo = tiposDeRecursosRepo;
            TiposDeCaracteristicasRepo = tiposDeCaracteriscasRepo;
            this.uowFactory = uowFactory;
        }

        //
        // GET: /Recursos/

        [Autorizar(TipoDeUsuario.Administrador)]
        public ActionResult Index(string orden, string filtroCodigo, string filtroTipo, string filtroNombre)
        {
            // Obtener orden
            ViewBag.OrdenNombre = string.IsNullOrEmpty(orden) ? "nombre_desc" : "";
            ViewBag.OrdenCodigo = orden == "codigo" ? "codigo_desc" : "codigo";
            ViewBag.OrdenTipo = orden == "tipo" ? "tipo_desc" : "tipo";

            var recursos = RecursosRepo.FiltrarYOrdenar(orden, filtroCodigo, filtroTipo, filtroNombre);

            return View(new ListadoRecursosVM(recursos, TiposDeRecursosRepo.Todos()));
        }

        //
        // GET: /Recursos/Details/5

        [Autorizar(TipoDeUsuario.Administrador)]
        public ActionResult Details(int id = 0)
        {
            Recurso recurso = RecursosRepo.ObtenerPorId(id);
            if (recurso == null)
            {
                return HttpNotFound();
            }
            return View(recurso);
        }

        //
        // GET: /Recursos/Create

        [Autorizar(TipoDeUsuario.Administrador)]
        public ActionResult Create()
        {
            var conversorRecurso = new ConversorRecurso(RecursosRepo, TiposDeRecursosRepo, TiposDeCaracteristicasRepo);

            return View(conversorRecurso.CrearViewModelVacio());
        }

        //
        // POST: /Recursos/Create

        [Autorizar(TipoDeUsuario.Administrador)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecursoVM recursoVM)
        {
            using (var uow = uowFactory.Actual)
            {
                var conversorRecurso = new ConversorRecurso(RecursosRepo, TiposDeRecursosRepo, TiposDeCaracteristicasRepo);

                Recurso nuevoRecurso = conversorRecurso.CrearDomainModel(recursoVM);

                var validador = new ValidadorDeRecursos(RecursosRepo, TiposDeRecursosRepo);

                if (ModelState.IsValid && validador.EsValido(nuevoRecurso))
                {
                    nuevoRecurso.Activar();

                    RecursosRepo.Agregar(nuevoRecurso);

                    uow.Commit();

                    return RedirectToAction("Index");
                }

                ModelStateHelper.CopyErrors(validador.Errores, ModelState);

                conversorRecurso.PoblarTiposDeRecursosSelectList(recursoVM);

                return View(recursoVM);
            }
        }

        //
        // GET: /Recursos/Edit/5

        [Autorizar(TipoDeUsuario.Administrador)]
        public ActionResult Edit(int id = 0)
        {
            Recurso recurso = RecursosRepo.ObtenerPorId(id);
            if (recurso == null)
            {
                return HttpNotFound();
            }

            var recursoVM = new ConversorRecurso(RecursosRepo, TiposDeRecursosRepo, TiposDeCaracteristicasRepo)
                .CrearViewModel(recurso);

            return View(recursoVM);
        }

        //
        // POST: /Recursos/Edit/5

        [Autorizar(TipoDeUsuario.Administrador)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RecursoVM recursoVM)
        {
            using (var uow = uowFactory.Actual)
            {
                var conversorRecurso = new ConversorRecurso(RecursosRepo, TiposDeRecursosRepo,
                    TiposDeCaracteristicasRepo);

                var recurso = conversorRecurso.ActualizarDomainModel(recursoVM);

                var validador = new ValidadorDeRecursos(RecursosRepo, TiposDeRecursosRepo);

                if (ModelState.IsValid && validador.EsValidoParaActualizar(recurso))
                {
                    RecursosRepo.Actualizar(recurso);

                    uow.Commit();

                    return RedirectToAction("Index");
                }

                ModelStateHelper.CopyErrors(validador.Errores, ModelState);

                conversorRecurso.PoblarTiposDeRecursosSelectList(recursoVM);

                return View(recursoVM);
            }
        }

        //
        // GET: /Recursos/Delete/5

        [Autorizar(TipoDeUsuario.Administrador)]
        public ActionResult Delete(int id = 0)
        {
            Recurso recurso = RecursosRepo.ObtenerPorId(id);
            if (recurso == null)
            {
                return HttpNotFound();
            }
            return View(recurso);
        }

        //
        // POST: /Recursos/Delete/5

        [Autorizar(TipoDeUsuario.Administrador)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var uow = uowFactory.Actual)
            {
                Recurso recurso = RecursosRepo.ObtenerPorId(id);

                recurso.Desactivar();

                RecursosRepo.Actualizar(recurso);

                uow.Commit();

                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Recursos/ObtenerTiposDeRecursosYCaracteristicas
        [Autorizar(TipoDeUsuario.Administrador)]
        public ActionResult ObtenerTiposDeRecursosYCaracteristicas()
        {
            var tipos = TiposDeRecursosRepo.Todos();

            if (tipos == null)
            {
                return HttpNotFound();
            }
            return Json(tipos, JsonRequestBehavior.AllowGet);
        }
    }
}