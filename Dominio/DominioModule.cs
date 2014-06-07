using Ninject.Modules;
using Ninject.Web.Common;

namespace Dominio
{
    public class DominioModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<IWeapon>().To<Sword>();
        }
    }
}
