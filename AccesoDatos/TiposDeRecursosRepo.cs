using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using System.Data;

namespace AccesoDatos
{
    public class TiposDeRecursosRepo
    {
            public ReservasContext Ctx { get; set; }

            public TiposDeRecursosRepo(ReservasContext ctx)
            {
                Ctx = ctx;
            }

            public IList<TipoRecurso> Todos()
            {
                return Ctx.TiposDeRecursos.ToList();
            }

            public TipoRecurso ObtenerPorId(int id)
            {
                return Ctx.TiposDeRecursos.Find(id);
            }

            public void Agregar(TipoRecurso tipoRecurso)
            {
                Ctx.TiposDeRecursos.Add(tipoRecurso);
                Ctx.SaveChanges();
            }

            public void Actualizar(TipoRecurso tipoRecurso)
            {
                Ctx.Entry(tipoRecurso).State = EntityState.Modified;
                Ctx.SaveChanges();
            }

            public void Eliminar(TipoRecurso tipoRecurso)
            {
                Ctx.TiposDeRecursos.Remove(tipoRecurso);
                Ctx.SaveChanges();
            }

        
    }
}
