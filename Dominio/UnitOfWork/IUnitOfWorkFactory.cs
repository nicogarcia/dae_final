
namespace Dominio.UnitOfWork
{
    /// <summary>
    /// Creador de Unit of Works
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// Devuelve la UnitOfWork que se está usando actualmente. Se corresponde con el request HTTP.
        /// </summary>
        IUnitOfWork Actual { get; }

    }
}
