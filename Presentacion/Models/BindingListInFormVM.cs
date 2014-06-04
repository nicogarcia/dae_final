using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentacion.Models
{
    public class BindingListInFormVM
    {
        public BindingListInFormVM()
        {
            this.Caracteristicas = new List<CaracteristicaVM>();
        }

        public string Nombre { get; set; }

        public IList<CaracteristicaVM> Caracteristicas { get; set; }

    }

    public class CaracteristicaVM
    {
        public bool Seleccionada { get; set; }
        public int Tipo { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }
}