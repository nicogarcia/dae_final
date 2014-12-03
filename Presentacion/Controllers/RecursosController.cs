using System;
using System.Net;
using Dominio.Entidades;
using Dominio.Queries;
using Dominio.Repos;
using Dominio.UnitOfWork;
using Dominio.Validacion;
using Presentacion.Filters;
using Presentacion.Models;
using System.Web.Mvc;
using Presentacion.Models.Conversores;
using Presentacion.Soporte;

namespace Presentacion.Controllers
{
    public class RecursosController : Controller
    {
        // Repos
        private IRecursosRepo RecursosRepo;
        private ITiposDeRecursosRepo TiposDeRecursosRepo;
        private ITiposDeCaracteristicasRepo TiposDeCaracteristicasRepo;
        
        // Repos queries
        private IRecursosQueriesTS RecursosQueries;
        private IMultipleTypeQueriesTS MultipleTypeQueries;

        // UoW
        private IUnitOfWorkFactory UowFactory;

        // Utilidades
        private IConversorRecurso ConversorRecurso;
        private IValidadorDeRecursos ValidadorDeRecursos;

        public RecursosController(
            IRecursosRepo recursosRepo, 
            ITiposDeCaracteristicasRepo tiposDeCaracteriscasRepo,
            ITiposDeRecursosRepo tiposDeRecursosRepo,
            IUnitOfWorkFactory uowFactory,
            IConversorRecurso conversor,
            IValidadorDeRecursos validador,
            IRecursosQueriesTS recursosQueries,
            IMultipleTypeQueriesTS multipleTypeQueries)
        {
            // Repos
            RecursosRepo = recursosRepo;
            TiposDeRecursosRepo = tiposDeRecursosRepo;
            TiposDeCaracteristicasRepo = tiposDeCaracteriscasRepo;

            // Repos queries
            RecursosQueries = recursosQueries;
            MultipleTypeQueries = multipleTypeQueries;
            
            // Utilidades
            ConversorRecurso = conversor;
            ValidadorDeRecursos = validador;

            // Unit of Work
            UowFactory = uowFactory;
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

            var recursos = RecursosQueries.FiltrarYOrdenar(orden, filtroCodigo, filtroTipo, filtroNombre);

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
            return View(ConversorRecurso.CrearViewModelVacio());
        }

        //
        // POST: /Recursos/Create

        [Autorizar(TipoDeUsuario.Administrador)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecursoVM recursoVM)
        {
            using (var uow = UowFactory.Actual)
            {
                Recurso nuevoRecurso = ConversorRecurso.CrearDomainModel(recursoVM);

                if (ModelState.IsValid && ValidadorDeRecursos.EsValido(nuevoRecurso))
                {
                    nuevoRecurso.Activar();

                    RecursosRepo.Agregar(nuevoRecurso);

                    uow.Commit();

                    return RedirectToAction("Index");
                }

                ModelStateHelper.CopyErrors(ValidadorDeRecursos.ObtenerErrores(), ModelState);

                ConversorRecurso.PoblarTiposDeRecursosSelectList(recursoVM);

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

            var recursoVM = ConversorRecurso.CrearViewModel(recurso);

            return View(recursoVM);
        }

        //
        // POST: /Recursos/Edit/5

        [Autorizar(TipoDeUsuario.Administrador)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RecursoVM recursoVM)
        {
            using (var uow = UowFactory.Actual)
            {
                var recurso = ConversorRecurso.ActualizarDomainModel(recursoVM);

                if (ModelState.IsValid && ValidadorDeRecursos.EsValidoParaActualizar(recurso))
                {
                    RecursosRepo.Actualizar(recurso);

                    uow.Commit();

                    return RedirectToAction("Index");
                }

                ModelStateHelper.CopyErrors(ValidadorDeRecursos.ObtenerErrores(), ModelState);

                ConversorRecurso.PoblarTiposDeRecursosSelectList(recursoVM);

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
            using (var uow = UowFactory.Actual)
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


        //
        // GET: /Recursos/BuscarRecurso
        [Autorizar]
        public ActionResult Buscar()
        {
            var busquedaRecursoVM = new BusquedaRecursoVM();

            ConversorRecurso.PoblarTiposDeRecursosSelectListConCampoVacio(busquedaRecursoVM);

            return View(busquedaRecursoVM);
        }

        //
        // POST: /Recursos/Buscar

        [Autorizar]
        [HttpPost, ActionName("Buscar")]
        [ValidateAntiForgeryToken]
        public ActionResult Buscar(BusquedaRecursoVM busquedaRecursoVM)
        {
            if (busquedaRecursoVM == null) 
                return HttpNotFound();

            var recursosDisponibles = MultipleTypeQueries.RecursosDisponibles(
                busquedaRecursoVM.Nombre,
                busquedaRecursoVM.Codigo,
                busquedaRecursoVM.TipoId,
                busquedaRecursoVM.CaracteristicasTipo,
                busquedaRecursoVM.CaracteristicasValor,
                busquedaRecursoVM.Inicio,
                busquedaRecursoVM.Fin
            );

            return Json(recursosDisponibles);
        }

        //
        // GET: /Recursos/ObtenerTodasCaracteristicas
        [Autorizar]
        public ActionResult ObtenerTodasCaracteristicas()
        {
            var tipos = TiposDeCaracteristicasRepo.Todos();

            if (tipos == null)
            {
                return HttpNotFound();
            }
            return Json(tipos, JsonRequestBehavior.AllowGet);
        }

    }
}