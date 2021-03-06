﻿using Ninject.Modules;
using Ninject.Web.Common;
using Presentacion.Filters;
using Presentacion.Models.Conversores;

namespace Presentacion
{
    public class PresentacionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IConversorRecurso>().To<ConversorRecurso>().InRequestScope();

            Bind<IConversorReserva>().To<ConversorReserva>().InRequestScope();

            Bind<IConversorUsuario>().To<ConversorUsuario>().InRequestScope();

            Bind<IWebSecuritySimpleWrapper>().To<WebSecuritySimpleWrapper>().InRequestScope();
        }
    }
}