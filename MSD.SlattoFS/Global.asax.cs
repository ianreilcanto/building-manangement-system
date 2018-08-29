﻿using MSD.SlattoFS.App_Start;
using System.Web.Mvc;
using System.Web.Routing;

namespace MSD.SlattoFS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //register dependencies 
            DependencyConfig.RegisterDependencies();
        }
    }
}
