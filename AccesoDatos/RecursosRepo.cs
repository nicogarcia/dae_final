using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dominio;

namespace AccesoDatos
{
    public class RecursosRepo
    {
        public ReservasContext Ctx { get; set; }

        public RecursosRepo(ReservasContext ctx)
        {
            Ctx = ctx;
        }

        public IList<Recurso> Todos()
        {
            return Ctx.Recursos.ToList();
        }

        public Recurso ObtenerPorId(int id)
        {
            return Ctx.Recursos.Find(id);
        }

        public void Agregar(Recurso recurso)
        {
            Ctx.Recursos.Add(recurso);
            Ctx.SaveChanges();
        }

        public void Actualizar(Recurso recurso)
        {
            Ctx.Entry(recurso).State = EntityState.Modified;
            Ctx.SaveChanges();
        }

        public void Eliminar(Recurso recurso)
        {
            Ctx.Recursos.Remove(recurso);
            Ctx.SaveChanges();
        }

        public IList<Recurso> FiltrarYOrdenar(string orden, string filtroCodigo, string filtroTipo, string filtroNombre)
        {
            // Query de recursos
            var recursos = Ctx.Recursos.AsQueryable();

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

    }
}