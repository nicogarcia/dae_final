﻿using System.Web.Mvc;
using Presentacion.Filters;

namespace Presentacion.Controllers
{
    [ExtendedHandleError(View = "Exception")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult IntentionalException()
        {
            var x = 0;
            x /= x;

            return View("Index");
        }
    }
}
