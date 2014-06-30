using Microsoft.Practices.ServiceLocation;

namespace Dominio.UnitOfWork
{
    /// <summary>
    /// Implementación del factory de UnitOfWork que delega la creación y obtención de la instancia en el 
    /// container de inyección de dependencias a través del ServiceLocator.
    /// </summary>
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        /// <summary>
        /// Devuelve la instancia de UnitOfWork del contexto del request HTTP.
        /// </summary>
        public IUnitOfWork Actual
        {
            get 
            {
                return ServiceLocator.Current.GetInstance<IUnitOfWork>(); 
            }
        }
    }
}
