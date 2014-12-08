using System;
using System.Data.Entity.Validation;
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
            // TODO: Remove extra stuff if unnecessary
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                ctx.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
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
