using System.Collections.Generic;

namespace Dominio
{
    public class TipoRecurso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public virtual ISet<TipoCaracteristica> TiposDeCaracteristicas { get; set; }
        
        public TipoRecurso() {
            TiposDeCaracteristicas = new HashSet<TipoCaracteristica>();
        }

        public TipoRecurso(string nombre) : this()
        {
            Nombre = nombre;
        }
    }
}
