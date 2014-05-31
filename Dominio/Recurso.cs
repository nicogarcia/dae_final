using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string result = "";

            result += "{ " + Nombre + ", ";

            result += "{ ";

            string separador = "";
            foreach (var caracteristica in Caracteristicas)
            {
                result += caracteristica.ToString() + separador;
                separador = ", ";
            }

            result += " } }";

            return result;
        }
    }
}
