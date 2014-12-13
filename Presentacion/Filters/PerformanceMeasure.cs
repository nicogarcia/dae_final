using System.Diagnostics;
using System.Web.Mvc;
using Ninject;
using Ninject.Extensions.Logging;

namespace Presentacion.Filters
{
    public class PerformanceMeasureAttribute : ActionFilterAttribute
    {
    }

    public class PerformanceMeasureFilter : IActionFilter
    {
        [Inject]
        public ILogger Logger { get; set; }

        private Stopwatch stopwatch;

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            stopwatch.Stop();
            Logger.Info("Action {0} from {1} response time: {2}ms", 
                new object[]
                {
                    filterContext.ActionDescriptor.ActionName, 
                    filterContext.Controller.ToString(),
                    stopwatch.ElapsedMilliseconds
                });
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            stopwatch = new Stopwatch();

            stopwatch.Start();
        }
    }
}