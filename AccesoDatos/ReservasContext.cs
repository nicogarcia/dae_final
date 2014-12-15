using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Dominio.Entidades;

namespace AccesoDatos
{
    /// <summary>
    /// Entity Framework DB Context
    /// </summary>
    public class ReservasContext : DbContext, IReservasContext
    {
        public ReservasContext()
            : base("Name=ReservasDB")
        { 
            Database.SetInitializer(new ReservasDbInitializer()); 
        }

        public IDbSet<Usuario> Usuarios { get; set; }
        public IDbSet<Reserva> Reservas { get; set; }

        public IDbSet<Recurso> Recursos { get; set; }
        public IDbSet<TipoRecurso> TiposDeRecursos { get; set; }
        public IDbSet<TipoCaracteristica> TiposDeCaracteristicas { get; set; }
        public IDbSet<Caracteristica> Caracteristicas { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            
            // Mapeo de la entidad Recurso
            builder.Entity<Recurso>().ToTable("Recursos");
            builder.Entity<Recurso>().HasMany<Caracteristica>(x => x.Caracteristicas);
            builder.Entity<Recurso>().HasRequired<TipoRecurso>(x => x.Tipo);

            // Mapeo de la entidad TipoRecurso
            builder.Entity<TipoRecurso>().ToTable("TiposDeRecursos");
            builder.Entity<TipoRecurso>()
                .HasMany<TipoCaracteristica>(x => x.TiposDeCaracteristicas)
                .WithMany();

            // Mapeo de la entidad Caracteristica
            builder.Entity<Caracteristica>().HasRequired<TipoCaracteristica>(x => x.Tipo);

            // Mapeo de la entidad TipoCaracteristica
            builder.Entity<TipoCaracteristica>().ToTable("TiposDeCaracteristicas");
        }
    }

}
