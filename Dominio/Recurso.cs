using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ninject.Infrastructure.Language;

namespace Dominio
{
    public class Recurso
    {
        public int Id { get; private set; }

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

        // TODO How to hide this? both getter and setter from user but not to EF
        public virtual ISet<Caracteristica> Caracteristicas { get; set; }

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

        public Recurso(string codigo, TipoRecurso tipo, string nombre, string descripcion) : this(nombre, tipo)
        {
            Codigo = codigo;
            Descripcion = descripcion;
        }

        public void AgregarCaracteristica(Caracteristica caracteristica)
        {
            Caracteristicas.Add(caracteristica);
        }

        public void AgregarCaracteristicas(IEnumerable<Caracteristica> caracteristicas)
        {
            Caracteristicas.UnionWith(caracteristicas);
        }

        public IEnumerable<Caracteristica> ObtenerCaracteristicas()
        {
            return Caracteristicas.ToEnumerable();
        }

        public void EliminarTodasCaracteristicas()
        {
            Caracteristicas.Clear();
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
