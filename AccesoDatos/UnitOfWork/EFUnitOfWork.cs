using Dominio.UnitOfWork;

namespace AccesoDatos.UnitOfWork
{
    /// <summary>
    /// Implmentación de UnitOfWork que recae en el contexto de Entity Framework
    /// </summary>
    public class EFUnitOfWork : IUnitOfWork
    {
        private ReservasContext ctx;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ctx">Contexto de EF</param>
        public EFUnitOfWork(ReservasContext ctx)
        {
            this.ctx = ctx;
        }

        /// <summary>
        /// Impacta los cambios sobre la BD.
        /// </summary>
        public void Commit()
        {
            ctx.SaveChanges();
        }

        /// <summary>
        /// Libera los recursos del contexto de EF.
        /// </summary>
        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}
