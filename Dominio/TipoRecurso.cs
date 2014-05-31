using System.Collections.Generic;

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
