using System.Web.Mvc;
using AccesoDatos;
using CommonServiceLocator.NinjectAdapter.Unofficial;
using Dominio;
using Microsoft.Practices.ServiceLocation;
using Ninject.MockingKernel.Moq;
using Ninject.Modules;
using Ninject.Web.Mvc.FilterBindingSyntax;
using Presentacion.App_Start;
using Presentacion.Filters;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ReservasDCICTest.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(ReservasDCICTest.App_Start.NinjectWebCommon), "Stop")]

namespace ReservasDCICTest.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new MoqMockingKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                // Initialize the ServciceLocator
                ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(kernel));

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(new INinjectModule[]
                {
                    new TestsModule()
                }
            );

            kernel.BindFilter<FiltroAutorizar>(FilterScope.Action, 0)
                .WhenActionMethodHas<AutorizarReservaAttribute>();
        }
    }
}
