using System.Collections.Generic;
using System.Linq;
using Dominio;
using System.Web.Mvc;

namespace Presentacion.Models
{
    public class ListadoRecursosVM
    {
        public IEnumerable<Recurso> Recursos { get; set; }
        public string TipoId { get; set; }
        public IEnumerable<TipoRecurso> TiposDeRecursos { get; private set; }
        public IEnumerable<SelectListItem> SelectTiposDeRecursos { get; set; }
        public IEnumerable<SelectListItem> TiposDeCaracteristicas { get; set; }

        public ListadoRecursosVM() { }

        public ListadoRecursosVM(IEnumerable<Recurso> recursos, IEnumerable<TipoRecurso> tiposDeRecursos)
        {
            TiposDeRecursos = tiposDeRecursos.ToArray();

            Recursos = recursos;

            SelectTiposDeRecursos = TiposDeRecursos.Select(
                tipo => new SelectListItem { Text = tipo.Nombre, Value = tipo.Id.ToString() });
        }
    }
}