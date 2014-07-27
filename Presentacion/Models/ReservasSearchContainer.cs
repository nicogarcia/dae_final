using Dominio;
using Dominio.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentacion.Models
{
    public class ReservasSearchContainer
    {
        IRecursosRepo repo_recursos { set; get; }
        public IList<ReservaVM> list { set; get; }
        public IEnumerable<SelectListItem> getStatesResources()
        {
            IList<SelectListItem> toR = new List<SelectListItem>();
            toR.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            foreach(string x in Enum.GetNames(typeof(EstadoRecurso)))
                toR.Add(new SelectListItem() { Text = x, Value = x});
            return toR;
        }
        public IEnumerable<SelectListItem> getResources()
        {
            IList<SelectListItem> toR = new List<SelectListItem>();
            toR.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            foreach (Recurso x in repo_recursos.Todos())
            {
                RecursoVM aux = new RecursoVM();
                toR.Add(new SelectListItem() { Text = x.Nombre + "-" + x.Tipo, Value = x.Nombre + "-" + x.Tipo });
            }
            return toR;
        }
    }
}