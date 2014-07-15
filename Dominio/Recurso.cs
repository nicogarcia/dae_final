using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ninject.Infrastructure.Language;

namespace Dominio
{
    public enum EstadoRecurso { Activo, Inactivo };
    
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

        public virtual TipoRecurso Tipo { get; set; }

        public virtual ISet<Caracteristica> Caracteristicas { get; set; }

        public EstadoRecurso EstadoActual { get; set; }

        public Recurso()
        {
            Caracteristicas = new HashSet<Caracteristica>();
            EstadoActual = EstadoRecurso.Inactivo;
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
            var caracteristicaMismoTipo = Caracteristicas.SingleOrDefault(c => c.Tipo == caracteristica.Tipo);

            if (caracteristicaMismoTipo != null)
                Caracteristicas.Remove(caracteristicaMismoTipo);

            Caracteristicas.Add(caracteristica);
        }

        public void AgregarCaracteristicas(IEnumerable<Caracteristica> caracteristicas)
        {
            foreach (var caracteristica in caracteristicas)
                AgregarCaracteristica(caracteristica);
        }

        public IEnumerable<Caracteristica> ObtenerCaracteristicas()
        {
            return Caracteristicas.ToEnumerable();
        }

        public void EliminarTodasCaracteristicas()
        {
            Caracteristicas.Clear();
        }

        public void Activar()
        {
            EstadoActual = EstadoRecurso.Activo;
        }

        public void Desactivar()
        {
            EstadoActual = EstadoRecurso.Inactivo;
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
