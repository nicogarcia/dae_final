using Ninject.Modules;
using Ninject.Web.Common;

namespace Presentacion.App_Start
{
    public class PresentacionModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<IWeapon>().To<Sword>();
        }
    }
}