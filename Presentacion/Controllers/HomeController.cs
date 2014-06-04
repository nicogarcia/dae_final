using Presentacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentacion.Controllers
{
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

        public ActionResult BindingListInForm()
        {
            var vm = new BindingListInFormVM();
            vm.Caracteristicas.Add(new CaracteristicaVM() { Nombre = "Caracteristica 1", Tipo = 1 });
            vm.Caracteristicas.Add(new CaracteristicaVM() { Nombre = "Caracteristica 2", Tipo = 2 });
            vm.Caracteristicas.Add(new CaracteristicaVM() { Nombre = "Caracteristica 3", Tipo = 3 });
            vm.Caracteristicas.Add(new CaracteristicaVM() { Nombre = "Caracteristica 4", Tipo = 4 });
            vm.Caracteristicas.Add(new CaracteristicaVM() { Nombre = "Caracteristica 5", Tipo = 5 });

            return View(vm);
        }

        [HttpPost]
        public ActionResult BindingListInForm(BindingListInFormVM vm)
        {


            return Redirect("Index");
        }

    }
}
