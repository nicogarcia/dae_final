using System.Linq;
using Dominio;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models
{
    public class RecursoVM
    {
        // Se usa en la creacion de un recurso
        public Recurso Recurso { get; set; }

        // Se usa para obtener los tipos de caracteristicas
        public TipoRecurso Tipo { get; private set; }

        public IEnumerable<TipoRecurso> TiposDeRecursos { get; private set; }
        public IEnumerable<SelectListItem> SelectTiposDeRecursos { get; set; }
        public IEnumerable<SelectListItem> TiposDeCaracteristicas { get; set; }

        /* TODO Lacks validation */
        public string Id { get; set; }
        [Required]
        public string Codigo { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<string> CaracteristicasValor { get; set; }
        public List<string> CaracteristicasTipo { get; set; }
        public string TipoId { get; set; }

        public RecursoVM() 
        {
            CaracteristicasTipo = new List<string>();
            CaracteristicasValor = new List<string>();
        }

        public RecursoVM(IEnumerable<TipoRecurso> tiposDeRecursos) : this()
        {
            TiposDeRecursos = tiposDeRecursos.ToArray();

            SelectTiposDeRecursos = TiposDeRecursos.Select(
                tipo => new SelectListItem { Text = tipo.Nombre, Value = tipo.Id.ToString() });

            Recurso = new Recurso();

            TiposDeCaracteristicas = TiposDeRecursos.First().TiposDeCaracteristicas.Select(
                caracteristica => new SelectListItem
                    { Text = caracteristica.Nombre, Value = caracteristica.Id.ToString() });
        }

        public RecursoVM(Recurso recurso, IEnumerable<TipoRecurso> tiposDeRecursos)
            : this(tiposDeRecursos)
        {
            //Recurso = recurso;

            Tipo = recurso.Tipo;
            Id = recurso.Id.ToString();

            // Cargar propiedades
            TipoId = recurso.Tipo.Id.ToString();
            Codigo = recurso.Codigo;
            Descripcion = recurso.Descripcion;
            Nombre = recurso.Nombre;

            // Agregar las caracteristicas actuales al modelo de vista
            CaracteristicasTipo.AddRange(recurso.Caracteristicas.Select(car => car.Tipo.Id.ToString()));
            CaracteristicasValor.AddRange(recurso.Caracteristicas.Select(car => car.Valor));
        }


    }
}