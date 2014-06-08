using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AccesoDatos.Repos;
using Dominio;
using AccesoDatos;
using Presentacion.Models;

namespace Presentacion.Controllers
{
    public class RecursosController : Controller
    {
        private ReservasContext db;
        private RecursosRepo recursosRepo;
        private TiposDeRecursosRepo tiposDeRecursosRepo;
        private TiposDeCaracteristicasRepo tiposDeCaracteristicasRepo;

        public RecursosController()
        {
            db = new ReservasContext();
            recursosRepo = new RecursosRepo(db);
            tiposDeRecursosRepo = new TiposDeRecursosRepo(db);
            tiposDeCaracteristicasRepo = new TiposDeCaracteristicasRepo(db);
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

            return View(new ListadoRecursosVM(recursos, tiposDeRecursosRepo.Todos()));
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
            return View(new RecursoVM(tiposDeRecursosRepo.Todos()));
        }

        //
        // POST: /Recursos/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecursoVM recursoVM)
        {
            if (!ModelState.IsValid)
            {
                // Repoblar tipos de recursos
                recursoVM.SelectTiposDeRecursos = tiposDeRecursosRepo.Todos()
                    .Select(tipo => new SelectListItem { Text = tipo.Nombre, Value = tipo.Id.ToString() });

                return View(recursoVM);
            }

            bool mismoCodigo = recursosRepo.Todos().Select(rec => rec.Codigo).Contains(recursoVM.Codigo);
            bool mismoNombre = recursosRepo.Todos().Select(rec => rec.Nombre).Contains(recursoVM.Nombre);

            if (mismoCodigo)
                ModelState.AddModelError("Recurso.Codigo", "El código de recurso ya existe.");
            if (mismoNombre)
                ModelState.AddModelError("Recurso.Nombre", "El nombre de recurso ya existe.");

            if (mismoCodigo || mismoNombre)
            {
                recursoVM.SelectTiposDeRecursos = tiposDeRecursosRepo.Todos().Select(
                    tipo => new SelectListItem { Text = tipo.Nombre, Value = tipo.Id.ToString() });

                return View(recursoVM);
            }

            // Construir objeto de dominio y cargar propiedades
            var recurso = new Recurso(
                recursoVM.Codigo,
                tiposDeRecursosRepo.ObtenerPorId(int.Parse(recursoVM.TipoId)),
                recursoVM.Nombre,
                recursoVM.Descripcion
            );

            // TODO Dangerous list sizes and not checking null
            // Cargar caracteristicas
            List<TipoCaracteristica> tiposDeCaracteristicas = recursoVM.CaracteristicasTipo
                .Select(tipo => tiposDeCaracteristicasRepo.ObtenerPorId(int.Parse(tipo))).Where(t => t != null).ToList();

            List<Caracteristica> caracteristicas = tiposDeCaracteristicas
                .Select((t, i) => new Caracteristica(t, recursoVM.CaracteristicasValor[i])).ToList();
            recurso.AgregarCaracteristicas(caracteristicas);

            // Marcar Recurso como Activo
            recurso.EstadoActual = Recurso.Estado.Activo;

            recursosRepo.Agregar(recurso);

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

            var recursoVM = new RecursoVM(recurso, tiposDeRecursosRepo.Todos());

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

            // TODO Dangerous list sizes and not checking null
            List<TipoCaracteristica> tiposDeCaracteristicas = recursoVM.CaracteristicasTipo
                .Select(tipo => tiposDeCaracteristicasRepo.ObtenerPorId(int.Parse(tipo))).Where(t => t != null).ToList();

            List<Caracteristica> caracteristicas = tiposDeCaracteristicas
                .Select((t, i) => new Caracteristica(t, recursoVM.CaracteristicasValor[i])).ToList();

            recurso.EliminarTodasCaracteristicas();
            recurso.AgregarCaracteristicas(caracteristicas);

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
        // TODO: Change result to JSON, parse in JS, Remove Partial view
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