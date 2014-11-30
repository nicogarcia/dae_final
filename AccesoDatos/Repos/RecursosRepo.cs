using System.Collections.Generic;
using System.Linq;
using Dominio;
using Dominio.Repos;

namespace AccesoDatos.Repos
{
    public class RecursosRepo : RepoBase<Recurso>, IRecursosRepo
    {


        public RecursosRepo(ReservasContext ctx)
            : base(ctx)
        {

        }

        public IList<Recurso> FiltrarYOrdenar(string orden, string filtroCodigo, string filtroTipo, string filtroNombre)
        {
            // Query de recursos
            IQueryable<Recurso> recursos = Filtrar(filtroCodigo, filtroNombre, filtroTipo);

            // Ordenar
            switch (orden)
            {
                case "nombre_desc":
                    recursos = recursos.OrderByDescending(recurso => recurso.Nombre);
                    break;
                case "codigo":
                    recursos = recursos.OrderBy(recurso => recurso.Codigo);
                    break;
                case "codigo_desc":
                    recursos = recursos.OrderByDescending(recurso => recurso.Codigo);
                    break;
                case "tipo":
                    recursos = recursos.OrderBy(recurso => recurso.Tipo.Nombre);
                    break;
                case "tipo_desc":
                    recursos = recursos.OrderByDescending(recurso => recurso.Tipo.Nombre);
                    break;
                default:
                    recursos = recursos.OrderBy(recurso => recurso.Nombre);
                    break;
            }

            return recursos.ToList();
        }

        public bool ExisteCodigo(string codigo)
        {
            return Ctx.Recursos.Any(recurso => recurso.Codigo == codigo);
        }

        public bool ExisteNombre(string nombre)
        {
            return Ctx.Recursos.Any(recurso => recurso.Nombre == nombre);
        }

        public Recurso ObtenerPorCodigo(string codigo)
        {
            return Ctx.Recursos.FirstOrDefault(recurso => recurso.Codigo == codigo);
        }

        public IList<Recurso> Buscar(string nombre, string codigo, string tipo, List<string> caracteristicasTipo,
            List<string> caracteristicasValor)
        {
            // Query de recursos
            IQueryable<Recurso> recursos = Filtrar(codigo, nombre, tipo);

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
                if(toBeAdded)
                    listaRecursos.Add(recurso);
            }

            return listaRecursos;
        }

        private IQueryable<Recurso> Filtrar(string filtroCodigo, string filtroNombre, string filtroTipo)
        {
            // Query de recursos
            IQueryable<Recurso> recursos = Ctx.Recursos;

            // No mostrar los recursos inactivos
            recursos = recursos.Where(recurso => recurso.EstadoActual == EstadoRecurso.Activo);

            // Aplicar filtros
            if (!string.IsNullOrEmpty(filtroCodigo))
                recursos = recursos.Where(recurso => recurso.Codigo.ToUpper().Contains(filtroCodigo.ToUpper()));

            if (!string.IsNullOrEmpty(filtroNombre))
                recursos = recursos.Where(recurso => recurso.Nombre.ToUpper().Contains(filtroNombre.ToUpper()));

            if (!string.IsNullOrEmpty(filtroTipo))
            {
                int filtroTipoInt = int.Parse(filtroTipo);
                recursos = recursos.Where(recurso => recurso.Tipo.Id == filtroTipoInt);
            }

            return recursos;
        }
    }
}