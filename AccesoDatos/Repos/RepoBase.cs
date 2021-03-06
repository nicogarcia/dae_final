﻿using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AccesoDatos.Repos
{
    public abstract class RepoBase<TTipo> where TTipo : class
    {
        protected IReservasContext Ctx { get; private set; }

        protected RepoBase(IReservasContext ctx)
        {
            Ctx = ctx;
        }

        public IList<TTipo> Todos()
        {
            return Ctx.Set<TTipo>().ToList();
        }

        public TTipo ObtenerPorId(int id)
        {
            return Ctx.Set<TTipo>().Find(id);
        }

        public void Agregar(TTipo entidad)
        {
            Ctx.Set<TTipo>().Add(entidad);
        }

        public void Actualizar(TTipo entidad)
        {
            Ctx.Entry(entidad).State = EntityState.Modified;
        }

        public void Eliminar(TTipo entidad)
        {
            Ctx.Set<TTipo>().Remove(entidad);
        }

        public IQueryable<TTipo> AsQueryable()
        {
            return Ctx.Set<TTipo>();
        } 
    }
}
