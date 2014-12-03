using Ninject.Modules;
using Ninject.Web.Common;
using Presentacion.Models.Conversores;

namespace Presentacion.App_Start
{
    public class PresentacionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IConversorRecurso>().To<ConversorRecurso>().InRequestScope();

            Bind<IConversorReservaMV>().To<ConversorReservaMV>().InRequestScope();
        }
    }
}