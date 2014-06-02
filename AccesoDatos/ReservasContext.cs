﻿using Dominio;
using System.Data.Entity;

namespace AccesoDatos
{
    /// <summary>
    /// Entity Framework DB Context
    /// </summary>
    public class ReservasContext : DbContext
    {
        public ReservasContext()
            : base("Name=ReservasDB")
        { 
            Database.SetInitializer(new ReservasDbInitializer()); 
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Recurso> Recursos { get; set; }
        public DbSet<TipoRecurso> TiposDeRecursos { get; set; }
        public DbSet<TipoCaracteristica> TiposDeCaracteristicas { get; set; }
        public DbSet<Caracteristica> Caracteristicas { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            // Mapeo de la entidad Recurso
            builder.Entity<Recurso>().ToTable("Recursos");
            builder.Entity<Recurso>().HasMany<Caracteristica>(x => x.Caracteristicas);
            builder.Entity<Recurso>().HasRequired<TipoRecurso>(x => x.Tipo);

            // Mapeo de la entidad TipoRecurso
            builder.Entity<TipoRecurso>().ToTable("TiposDeRecursos");
            builder.Entity<TipoRecurso>().HasMany<TipoCaracteristica>(x => x.TiposDeCaracteristicas);

            // Mapeo de la entidad Caracteristica
            builder.Entity<Caracteristica>().HasRequired<TipoCaracteristica>(x => x.Tipo);

            // Mapeo de la entidad TipoCaracteristica
            builder.Entity<TipoCaracteristica>().ToTable("TiposDeCaracteristicas");
        }
    }

}
