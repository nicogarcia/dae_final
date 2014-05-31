using System.Collections.Generic;

namespace Dominio
{
    public class Recurso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public TipoRecurso Tipo { get; set; }
        public ISet<Caracteristica> Caracteristicas { get; set; }/*TODO validation setter*/

        public Recurso() {
            Caracteristicas = new HashSet<Caracteristica>();
        }

        public Recurso(string nombre, TipoRecurso tipo) : this()
        {
            Nombre = nombre;
            Tipo = tipo;
        }

        public override string ToString()
        {
            var result = "{ " + Nombre + ", {";

            var separador = "";
            foreach (var caracteristica in Caracteristicas)
            {
                result += separador + caracteristica.ToString();
                separador = ", ";
            }

            result += " } }";

            return result;
        }
    }
}
