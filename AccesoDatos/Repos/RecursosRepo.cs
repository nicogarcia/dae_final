using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
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
            return Todos().Select(recurso => recurso.Codigo).Contains(codigo);
        }

        public bool ExisteNombre(string nombre)
        {
            return Todos().Select(recurso => recurso.Nombre).Contains(nombre);
        }

        public Recurso BuscarRecurso(string codigo)
        {
            IQueryable<Recurso> consulta = Ctx.Recursos;
            IList<Recurso> listado = consulta.Where(r => r.Codigo == codigo).ToList();
            if (listado.Count != 0)
                return listado.First();
            else
                return null;
        }


        public IList<Recurso> Buscar(string nombre, string codigo, string tipo, List<string> caracteristicasTipo,
            List<string> caracteristicasValor)
        {
            // Query de recursos
            IQueryable<Recurso> recursos = Filtrar(codigo, nombre, tipo);

            // Filtrar por caracteristicas
            var listaRecursos = new List<Recurso>();
            
            foreach (var recurso in recursos.ToList())
            {
                int i = 0;
                bool add = true;
                foreach (var caracteristicaTipo in caracteristicasTipo)
                {
                    if (!recurso.Caracteristicas.Any(
                        caracteristica => caracteristica.Tipo.Id.ToString() == caracteristicaTipo &&
                            caracteristica.Valor == caracteristicasValor[i]))
                    {
                        add = false;
                        break;
                    }
                    i++;
                }
                if(add)
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
                recursos = recursos.Where(r => r.Codigo.ToUpper().Contains(filtroCodigo.ToUpper()));

            if (!string.IsNullOrEmpty(filtroNombre))
                recursos = recursos.Where(r => r.Nombre.ToUpper().Contains(filtroNombre.ToUpper()));

            if (!string.IsNullOrEmpty(filtroTipo))
            {
                int filtroTipoInt = int.Parse(filtroTipo);
                recursos = recursos.Where(r => r.Tipo.Id == filtroTipoInt);
            }

            return recursos;
        }
    }
}