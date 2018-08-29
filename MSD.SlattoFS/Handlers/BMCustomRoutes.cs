using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Umbraco.Core.Logging;

namespace MSD.SlattoFS.Handlers
{
    public static class BMCustomRoutes
    {
        public static void RegisterRoutes()
        {
            LogHelper.Info(typeof(BMCustomRoutes), "Initializing Custom Routes");

            RouteTable.Routes.MapRoute(
               "DeleteAddress",
               "BMBuilding/DeleteAddress/{addressId}/{buildingId}",
               new { controller = "BMBuilding", action = "DeleteAddress"}
               );

            RouteTable.Routes.MapRoute(
              "UpdateBuilding",
              "BMBuilding/UpdateBuilding/",
              new { controller = "BMBuilding", action = "UpdateBuilding" }
              );

            RouteTable.Routes.MapRoute(
              "AddBuilding",
              "BMBuilding/AddBuilding/",
              new { controller = "BMBuilding", action = "AddBuilding" }
              );

            RouteTable.Routes.MapRoute(
              "DeleteBuilding",
              "BMBuilding/DeleteBuilding/",
              new { controller = "BMBuilding", action = "DeleteBuilding" }
              );
        }
    }
}