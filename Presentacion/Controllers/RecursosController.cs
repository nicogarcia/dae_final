using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Dominio;
using AccesoDatos;
using DotNetOpenAuth.Messaging;
using Presentacion.Models;

namespace Presentacion.Controllers
{
    public class RecursosController : Controller
    {
        /* TODO: Use Repository pattern */
        private ReservasContext db;
        private RecursosRepo recursosRepo;
        private TiposDeRecursosRepo tiposDeRecursosRepo;

        public RecursosController()
        {
            db = new ReservasContext();
            recursosRepo = new RecursosRepo(db);
            tiposDeRecursosRepo = new TiposDeRecursosRepo(db);
        }

        //
        // GET: /Recursos/

        public ActionResult Index(string orden, string filtroCodigo, string filtroTipo, string filtroNombre)
        {
            // Obtener orden
            ViewBag.OrdenNombre = string.IsNullOrEmpty(orden) ? "nombre_desc" : "";
            ViewBag.OrdenCodigo = orden == "codigo" ? "codigo_desc" : "codigo";
            ViewBag.OrdenTipo = orden == "tipo" ? "tipo_desc" : "tipo";
            
            var recursos = recursosRepo.FiltrarYOrdenar(orden, filtroCodigo, filtroTipo, filtroNombre);

            IEnumerable<TipoRecurso> tiposDeRecursos = tiposDeRecursosRepo.Todos();

            return View(new ListadoRecursosVM(recursos, tiposDeRecursos));
        }

        //
        // GET: /Recursos/Details/5

        public ActionResult Details(int id = 0)
        {
            Recurso recurso = recursosRepo.ObtenerPorId(id);
            if (recurso == null)
            {
                return HttpNotFound();
            }
            return View(recurso);
        }

        //
        // GET: /Recursos/Create

        public ActionResult Create()
        {
            // Se cargan los tipos de recursos desde el contexto.
            IEnumerable<TipoRecurso> tiposDeRecursos = tiposDeRecursosRepo.Todos();

            return View(new RecursoVM(tiposDeRecursos));
        }

        //
        // POST: /Recursos/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecursoVM recursoVM)
        {
            /* TODO Removed ModelState.IsValid, is that correct? */
            if (recursoVM == null) return View();

            bool mismoCodigo = recursosRepo.Todos().Select(rec => rec.Codigo).Contains(recursoVM.Codigo);
            bool mismoNombre = recursosRepo.Todos().Select(rec => rec.Nombre).Contains(recursoVM.Nombre);

            if (mismoCodigo) ModelState.AddModelError("Recurso.Codigo", "El código de recurso ya existe.");
            if (mismoNombre) ModelState.AddModelError("Recurso.Nombre", "El nombre de recurso ya existe.");

            if (mismoCodigo || mismoNombre)
            {
                IEnumerable<TipoRecurso> tiposDeRecursos = tiposDeRecursosRepo.Todos();
             
                recursoVM.SelectTiposDeRecursos = tiposDeRecursos.Select(
                    tipo => new SelectListItem { Text = tipo.Nombre, Value = tipo.Id.ToString() });
                
                return View(recursoVM);
            }

            // Construir objeto de dominio y cargar propiedades
            var recurso = new Recurso
            {
                Tipo = tiposDeRecursosRepo.ObtenerPorId(int.Parse(recursoVM.TipoId)),
                Codigo = recursoVM.Codigo,
                Descripcion = recursoVM.Descripcion,
                Nombre = recursoVM.Nombre
            };

            // TODO Ask for TiposDeCaracteristicas repo
            // TODO Dangerous list sizes and not checking null
            // Cargar caracteristicas
            List<TipoCaracteristica> tiposDeCaracteristicas = recursoVM.CaracteristicasTipo
                .Select(tipo => db.TiposDeCaracteristicas.Find(int.Parse(tipo))).Where(t => t != null).ToList();

            List<Caracteristica> caracteristicas = tiposDeCaracteristicas
                .Select((t, i) => new Caracteristica(t, recursoVM.CaracteristicasValor[i])).ToList();
            recurso.Caracteristicas.AddRange(caracteristicas);

            // Marcar Recurso como Activo
            recurso.EstadoActual = Recurso.Estado.Activo;

            recursosRepo.Agregar(recursoVM.Recurso);

            return RedirectToAction("Index");
        }

        //
        // GET: /Recursos/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Recurso recurso = recursosRepo.ObtenerPorId(id);
            if (recurso == null)
            {
                return HttpNotFound();
            }

            IEnumerable<TipoRecurso> tiposDeRecursos = tiposDeRecursosRepo.Todos();
            var recursoVM = new RecursoVM(recurso, tiposDeRecursos) { TipoId = recurso.Tipo.Id.ToString() };

            return View(recursoVM);
        }

        //
        // POST: /Recursos/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RecursoVM recursoVM)
        {
            /* TODO Removed ModelState.IsValid, is that correct? */
            if (recursoVM == null) return View();

            // Validar Nombre y Codigo
            bool mismoCodigo = recursosRepo.Todos()
                .Where(rec => (rec.Codigo.Equals(recursoVM.Codigo)) && (rec.Id != int.Parse(recursoVM.Id)))
                .ToList().Count != 0;
            bool mismoNombre = recursosRepo.Todos()
                .Where(rec => (rec.Nombre.Equals(recursoVM.Nombre)) && (rec.Id != int.Parse(recursoVM.Id)))
                .ToList().Count != 0;

            if (mismoCodigo) ModelState.AddModelError("Recurso.Codigo", "El código de recurso ya existe.");
            if (mismoNombre) ModelState.AddModelError("Recurso.Nombre", "El nombre de recurso ya existe.");

            if (mismoCodigo || mismoNombre)
            {
                IEnumerable<TipoRecurso> tiposDeRecursos = tiposDeRecursosRepo.Todos();

                recursoVM.SelectTiposDeRecursos = tiposDeRecursos.Select(
                    tipo => new SelectListItem { Text = tipo.Nombre, Value = tipo.Id.ToString() });

                return View(recursoVM);
            }

            var recurso = recursosRepo.ObtenerPorId(int.Parse(recursoVM.Id));

            // Construir objeto de dominio y persistir
            recurso.Tipo = tiposDeRecursosRepo.ObtenerPorId(int.Parse(recursoVM.TipoId));
            recurso.Codigo = recursoVM.Codigo;
            recurso.Descripcion = recursoVM.Descripcion;
            recurso.Nombre = recursoVM.Nombre;

            // TODO Ask for TiposDeCaracteristicas repo
            List<TipoCaracteristica> tiposDeCaracteristicas = recursoVM.CaracteristicasTipo
                .Select(tipo => db.TiposDeCaracteristicas.Find(int.Parse(tipo))).Where(t => t != null).ToList();

            List<Caracteristica> caracteristicas = tiposDeCaracteristicas
                .Select((t, i) => new Caracteristica(t, recursoVM.CaracteristicasValor[i])).ToList();

            recurso.Caracteristicas.Clear();
            recurso.Caracteristicas.AddRange(caracteristicas);
            
            recursosRepo.Actualizar(recurso);

            return RedirectToAction("Index");
        }

        //
        // GET: /Recursos/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Recurso recurso = recursosRepo.ObtenerPorId(id);
            if (recurso == null)
            {
                return HttpNotFound();
            }
            return View(recurso);
        }

        //
        // POST: /Recursos/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recurso recurso = recursosRepo.ObtenerPorId(id);

            recurso.EstadoActual = Recurso.Estado.Inactivo;

            recursosRepo.Actualizar(recurso);

            return RedirectToAction("Index");
        }

        //
        // GET: /Recursos/ObtenerTiposPartial/5

        public ActionResult ObtenerTiposPartial(int tipoId = 0)
        {
            var tipo = tiposDeRecursosRepo.ObtenerPorId(tipoId);

            if (tipo == null)
            {
                return HttpNotFound();
            }
            return PartialView(tipo);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}