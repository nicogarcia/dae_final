using System;
using System.Web;
using System.Web.Mvc;
using AccesoDatos;
using CommonServiceLocator.NinjectAdapter.Unofficial;
using Dominio;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Ninject.Web.Mvc.FilterBindingSyntax;
using Presentacion;
using Presentacion.Filters;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace Presentacion
{
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
            var kernel = new StandardKernel();
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
                new AccesoDatosModule(), 
                new DominioModule(), 
                new PresentacionModule() 
            });

            kernel.BindFilter<FiltroAutorizarReserva>(FilterScope.Action, 0)
                .WhenActionMethodHas<AutorizarReservaAttribute>();

            kernel.BindFilter<PerformanceMeasureFilter>(FilterScope.Action, 0)
                .WhenActionMethodHas<PerformanceMeasureAttribute>();
        }        
    }
}
