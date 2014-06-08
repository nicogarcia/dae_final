using System.Linq;
using Dominio;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models
{
    public class RecursoVM
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Codigo { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(250)]
        public string Descripcion { get; set; }

        [Required]
        public string TipoId { get; set; }

        public List<string> CaracteristicasValor { get; set; }
        public List<string> CaracteristicasTipo { get; set; }
        
        public IEnumerable<TipoRecurso> TiposDeRecursos { get; private set; }
        public IEnumerable<SelectListItem> SelectTiposDeRecursos { get; set; }
        public IEnumerable<SelectListItem> TiposDeCaracteristicas { get; set; }

        // TODO: To be removed, used to get Initial set of TipoCaracteristica
        public TipoRecurso Tipo { get; set; }

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
            
            TiposDeCaracteristicas = TiposDeRecursos.First().TiposDeCaracteristicas.Select(
                caracteristica => new SelectListItem
                    { Text = caracteristica.Nombre, Value = caracteristica.Id.ToString() });
        }

        public RecursoVM(Recurso recurso, IEnumerable<TipoRecurso> tiposDeRecursos)
            : this(tiposDeRecursos)
        {
            // Cargar propiedades
            Tipo = recurso.Tipo;
            Id = recurso.Id.ToString();
            TipoId = recurso.Tipo.Id.ToString();
            Codigo = recurso.Codigo;
            Descripcion = recurso.Descripcion;
            Nombre = recurso.Nombre;

            // Agregar las caracteristicas actuales al modelo de vista
            CaracteristicasTipo.AddRange(recurso.ObtenerCaracteristicas().Select(car => car.Tipo.Id.ToString()));
            CaracteristicasValor.AddRange(recurso.ObtenerCaracteristicas().Select(car => car.Valor));
        }


    }
}