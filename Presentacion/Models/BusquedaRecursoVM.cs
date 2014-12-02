using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dominio;
using Dominio.Entidades;

namespace Presentacion.Models
{
    public class BusquedaRecursoVM
    {
        [MaxLength(10)]
        public string Codigo { get; set; }

        [MaxLength(50)]
        public string Nombre { get; set; }
        
        public string TipoId { get; set; }

        public List<string> CaracteristicasValor { get; set; }
        public List<string> CaracteristicasTipo { get; set; }

        public IEnumerable<TipoRecurso> TiposDeRecursos { get; private set; }
        public IEnumerable<SelectListItem> SelectTiposDeRecursos { get; set; }

        public DateTime Inicio { get; set; }

        public DateTime Fin { get; set; }

        public BusquedaRecursoVM()
        {
            CaracteristicasTipo = new List<string>();
            CaracteristicasValor = new List<string>();
        }
    }
}