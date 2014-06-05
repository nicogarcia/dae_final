using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dominio
{
    public class Recurso
    {
        public int Id { get; set; }

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
        public virtual TipoRecurso Tipo { get; set; }
        
        public virtual ISet<Caracteristica> Caracteristicas { get; set; }/*TODO validate on setter*/

        public enum Estado { Activo, Inactivo };

        public Estado EstadoActual { get; set; }

        public Recurso() {
            Caracteristicas = new HashSet<Caracteristica>();
            EstadoActual = Estado.Inactivo;
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
