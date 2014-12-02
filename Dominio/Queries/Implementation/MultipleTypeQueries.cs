using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Entidades;
using Dominio.Repos;

namespace Dominio.Queries.Implementation
{
    public class MultipleQueriesTS : IMultipleTypeQueriesTS
    {
        private IReservasRepo ReservasRepo;
        private IRecursosQueriesTS RecursosQueries;

        public MultipleQueriesTS(IRecursosQueriesTS recursosQueries, IReservasRepo reservasRepo)
        {
            RecursosQueries = recursosQueries;
            ReservasRepo = reservasRepo;
        }

        public IEnumerable<Recurso> RecursosDisponibles(
            string nombre,
            string codigo,
            string tipo,
            List<string> caracteristicasTipo,
            List<string> caracteristicasValor,
            DateTime inicio,
            DateTime fin)
        {
            var recursosQuery = Buscar(nombre, codigo, tipo, caracteristicasTipo, caracteristicasValor);

            IQueryable<Recurso> noDisponibles =
                ReservasRepo.AsQueryable()
                .Where(reserva =>
                    (reserva.Inicio >= inicio && reserva.Inicio <= fin) || (reserva.Fin >= inicio && reserva.Fin <= fin)
                )
                .Select(reserva => reserva.RecursoReservado);

            return recursosQuery.Except(noDisponibles).ToList();
        }

        private List<Recurso> Buscar(
            string nombre,
            string codigo,
            string tipo,
            List<string> caracteristicasTipo,
            List<string> caracteristicasValor)
        {
            // Query de recursos
            IQueryable<Recurso> recursos = RecursosQueries.Filtrar(codigo, nombre, tipo);

            // Filtrar por caracteristicas

            // Lista de recursos validos
            var listaRecursos = new List<Recurso>();

            foreach (var recurso in recursos.ToList())
            {
                int i = 0;
                bool toBeAdded = true;
                foreach (var caracteristicaTipo in caracteristicasTipo)
                {
                    bool noCumpleCaracteristica = !recurso.Caracteristicas.Any(
                        caracteristica => caracteristica.Tipo.Id.ToString() == caracteristicaTipo &&
                                          caracteristica.Valor.ToUpper().Contains(caracteristicasValor[i].ToUpper()));



                    // Si existe alguno que no cumple con el tipo y valor de las
                    // caracteristicas de filtro no se agregara a la lista
                    if (noCumpleCaracteristica)
                    {
                        toBeAdded = false;
                        break;
                    }
                    i++;
                }

                // Si no fue descartado, el recurso es valido
                if (toBeAdded)
                    listaRecursos.Add(recurso);
            }

            return listaRecursos;
        }
    }
}
