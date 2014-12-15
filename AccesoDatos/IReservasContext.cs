using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Dominio.Entidades;

namespace AccesoDatos
{
    public interface IReservasContext
    {
        IDbSet<Usuario> Usuarios { get; set; }
        IDbSet<Reserva> Reservas { get; set; }
        IDbSet<Recurso> Recursos { get; set; }
        IDbSet<TipoRecurso> TiposDeRecursos { get; set; }
        IDbSet<TipoCaracteristica> TiposDeCaracteristicas { get; set; }
        IDbSet<Caracteristica> Caracteristicas { get; set; }
        
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry Entry(object entity);

        int SaveChanges();

        void Dispose();
    }
}