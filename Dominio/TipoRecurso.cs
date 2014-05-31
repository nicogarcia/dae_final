using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio
{
    public class TipoRecurso
    {
        public int Id { get; set; }
        public ISet<TipoCaracteristica> TiposDeCaracteristicas { get; set; }
        
        public TipoRecurso() {
            TiposDeCaracteristicas = new HashSet<TipoCaracteristica>();
        }
    }
}
