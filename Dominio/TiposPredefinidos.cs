using System.Collections.Generic;

namespace Dominio
{
    public class TiposPredefinidos
    {
        public static IEnumerable<TipoRecurso> ObtenerTiposPredefinidos()
        {
            var coleccion = new List<TipoRecurso>();

            // Tipos de Recursos predefinidos
            var aula = new TipoRecurso();
            var laboratorio = new TipoRecurso();
            var proyector = new TipoRecurso();

            // Tipos de caracteristicas comunes a Aulas y Laboratorios
            var capacidad = new TipoCaracteristica("Capacidad");
            var edificio = new TipoCaracteristica("Edificio");
            var tipoPizarron = new TipoCaracteristica("Tipo de Pizarrón");
            var cantidadDePizarrones = new TipoCaracteristica("Cantidad de Pizarrones");

            // Tipos de caracteristicas de dispositivos

            //      Aula
            aula.TiposDeCaracteristicas.Add(capacidad);
            aula.TiposDeCaracteristicas.Add(edificio);
            aula.TiposDeCaracteristicas.Add(tipoPizarron);
            aula.TiposDeCaracteristicas.Add(cantidadDePizarrones);

            //      Laboratorio
            laboratorio.TiposDeCaracteristicas.Add(capacidad);
            laboratorio.TiposDeCaracteristicas.Add(edificio);
            laboratorio.TiposDeCaracteristicas.Add(tipoPizarron);
            laboratorio.TiposDeCaracteristicas.Add(cantidadDePizarrones);

            //      Proyector
            proyector.TiposDeCaracteristicas.Add(new TipoCaracteristica("Marca"));
            proyector.TiposDeCaracteristicas.Add(new TipoCaracteristica("Resolución"));

            coleccion.AddRange(new [] { aula, laboratorio, proyector});

            return coleccion;
        }
    }
}
