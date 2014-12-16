using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Dominio.Entidades;

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

        public TipoRecurso Tipo { get; set; }

        public RecursoVM()
        {
            CaracteristicasTipo = new List<string>();
            CaracteristicasValor = new List<string>();
        }

        public RecursoVM(string id, string codigo, string nombre, string descripcion, string tipoId) : this()
        {
            Id = id;
            TipoId = tipoId;
            Codigo = codigo;
            Descripcion = descripcion;
            Nombre = nombre;
        }
    }
}