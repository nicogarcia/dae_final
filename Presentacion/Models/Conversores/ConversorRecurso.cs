using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using Dominio;
using Dominio.Entidades;
using Dominio.Repos;

namespace Presentacion.Models.Conversores
{
    public class ConversorRecurso : IConversorRecurso
    {
        public TipoRecurso[] TiposDeRecursos;
        public ITiposDeRecursosRepo TiposDeRecursosRepo;
        public ITiposDeCaracteristicasRepo TiposDeCaracteristicasRepo;
        public IRecursosRepo RecursosRepo;

        public ConversorRecurso(IRecursosRepo recursosRepo, ITiposDeRecursosRepo tiposDeRecursosRepo,
            ITiposDeCaracteristicasRepo tiposDeCaracteristicasRepo)
        {
            RecursosRepo = recursosRepo;
            TiposDeRecursosRepo = tiposDeRecursosRepo;
            TiposDeCaracteristicasRepo = tiposDeCaracteristicasRepo;
        }

        public RecursoVM CrearViewModelVacio()
        {
            var recursoVM = new RecursoVM();

            PoblarTiposDeRecursosSelectList(recursoVM);

            return recursoVM;
        }

        public virtual RecursoVM CrearViewModel(Recurso recurso)
        {
            var recursoVM = new RecursoVM(
                recurso.Id.ToString(),
                recurso.Codigo,
                recurso.Nombre,
                recurso.Descripcion,
                recurso.Tipo.Id.ToString()
            );

            recursoVM.Tipo = recurso.Tipo;

            PoblarTiposDeRecursosSelectList(recursoVM);

            // Agregar las caracteristicas actuales al modelo de vista
            recursoVM.CaracteristicasTipo.AddRange(recurso.ObtenerCaracteristicas().Select(car => car.Tipo.Id.ToString()));
            recursoVM.CaracteristicasValor.AddRange(recurso.ObtenerCaracteristicas().Select(car => car.Valor));

            return recursoVM;
        }

        public Recurso CrearDomainModel(RecursoVM recursoVM)
        {
            var recurso = new Recurso(
                        recursoVM.Codigo,
                        TiposDeRecursosRepo.ObtenerPorId(int.Parse(recursoVM.TipoId)),
                        recursoVM.Nombre,
                        recursoVM.Descripcion
                    );

            recurso.EliminarTodasCaracteristicas();

            CargarCaracteristicas(recurso, recursoVM);

            return recurso;
        }

        public Recurso ActualizarDomainModel(RecursoVM recursoVM)
        {
            Recurso recurso = RecursosRepo.ObtenerPorId(int.Parse(recursoVM.Id));

            recurso.Tipo = TiposDeRecursosRepo.ObtenerPorId(int.Parse(recursoVM.TipoId));
            recurso.Codigo = recursoVM.Codigo;
            recurso.Descripcion = recursoVM.Descripcion;
            recurso.Nombre = recursoVM.Nombre;

            recurso.EliminarTodasCaracteristicas();

            CargarCaracteristicas(recurso, recursoVM);

            return recurso;
        }

        private void CargarCaracteristicas(Recurso recurso, RecursoVM recursoVM)
        {
            List<TipoCaracteristica> tiposDeCaracteristicas = recursoVM.CaracteristicasTipo
                .Select(tipo => TiposDeCaracteristicasRepo.ObtenerPorId(int.Parse(tipo)))
                .Where(tipo => tipo != null)
                .ToList();

            List<Caracteristica> caracteristicas = tiposDeCaracteristicas
                .Select((tipo, i) => new Caracteristica(tipo, recursoVM.CaracteristicasValor[i]))
                .Where(caract => !caract.Valor.IsEmpty())
                .ToList();

            recurso.AgregarCaracteristicas(caracteristicas);
        }

        public void PoblarTiposDeRecursosSelectList(RecursoVM recursoVM)
        {
            recursoVM.SelectTiposDeRecursos = TiposDeRecursosRepo.Todos()
                .Select(tipo => new SelectListItem { Text = tipo.Nombre, Value = tipo.Id.ToString() });
        }

        public void PoblarTiposDeRecursosSelectList(BusquedaRecursoVM busquedaRecursoVM)
        {
            busquedaRecursoVM.SelectTiposDeRecursos = TiposDeRecursosRepo.Todos()
                .Select(tipo => new SelectListItem { Text = tipo.Nombre, Value = tipo.Id.ToString() });
        }

        public void PoblarTiposDeRecursosSelectListConCampoVacio(BusquedaRecursoVM busquedaRecursoVM)
        {
            var listaTiposRecursos = new List<SelectListItem>();

            listaTiposRecursos.Add(new SelectListItem() { Text = "", Value = "", Selected = true });

            listaTiposRecursos.AddRange(TiposDeRecursosRepo.Todos()
                .Select(tipo => new SelectListItem { Text = tipo.Nombre, Value = tipo.Id.ToString() }));

            busquedaRecursoVM.SelectTiposDeRecursos = listaTiposRecursos;
        }
    }
}