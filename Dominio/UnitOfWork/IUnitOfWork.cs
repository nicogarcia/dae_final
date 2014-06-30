using System;

namespace Dominio.UnitOfWork
{
    /// <summary>
    /// Abstracción del patrón UnitOfWork
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Ejecuta la transacción.
        /// </summary>
        void Commit();
    }
}
