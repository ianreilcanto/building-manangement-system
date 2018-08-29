using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;

namespace MSD.SlattoFS.Handlers
{
    public class BMRouteConfiguration
    {
        public static void Initialize(ApplicationContext appContext)
        {
            RouteTable.Routes.MapRoute(
               "BuildingAssetUpload",
               "BMBuilding/BMBuildingUploadAssets",
               new
               {
                   controller = "BMBuilding",
                   action = "BMBuildingUploadAssets"
               });

            RouteTable.Routes.MapRoute(
               "BuildingAssetList",
               "BMBuilding/BMBuildingAssets",
               new
               {
                   controller = "BMBuilding",
                   action = "BMBuildingAssets"
                   //id = UrlParameter.Optional
               });

            RouteTable.Routes.MapRoute(
               "BuildingCreate",
               "BMBuilding/AddBuilding",
               new
               {
                   controller = "BMBuilding",
                   action = "AddBuilding"
               });

            RouteTable.Routes.MapRoute(
               "BMBuildingRemoveAsset",
               "BMBuilding/BMBuildingRemoveAsset",
               new
               {
                   controller = "BMBuilding",
                   action = "BMBuildingRemoveAsset"
               });

            RouteTable.Routes.MapRoute(
               "BMBuildingSortAssets",
               "BMBuilding/BMBuildingSortAssets",
               new
               {
                   controller = "BMBuilding",
                   action = "BMBuildingSortAssets"
               });

            RouteTable.Routes.MapRoute(
               "bmbuildinguploadinfo",
               "BMBuilding/bmbuildinguploadinfo",
               new
               {
                   controller = "BMBuilding",
                   action = "bmbuildinguploadinfo"
               });

            RouteTable.Routes.MapRoute(
               "bmbuildinginfo",
               "BMBuilding/bmbuildinginfo",
               new
               {
                   controller = "BMBuilding",
                   action = "bmbuildinginfo"
               });

            RouteTable.Routes.MapRoute(
               "bmbuildingapartmentuploadinfo",
               "BMBuilding/bmbuildingapartmentuploadinfo",
               new
               {
                   controller = "BMBuilding",
                   action = "bmbuildingapartmentuploadinfo"
               });

            RouteTable.Routes.MapRoute(
              "DeleteAddress",
              "BMBuilding/DeleteAddress/{addressId}/{buildingId}",
              new { controller = "BMBuilding", action = "DeleteAddress" }
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

            RouteTable.Routes.MapRoute(
               "DownloadApartmentPDF",
               "BMBuilding/DownloadApartmentPDF/{apartmentId}",
               new { controller = "BMBuilding", action = "DownloadApartmentPDF" }
               );

            RouteTable.Routes.MapRoute(
                "SaveSvgData",
                "BMBuilding/SaveSvgData",
                new { controller = "BMBuilding", action = "SaveSvgData" }
            );

            RouteTable.Routes.MapRoute(
                "GetSvgData",
                "BMBuilding/GetSvgData",
                new { controller = "BMBuilding", action = "GetSvgData" }
            );


            RouteTable.Routes.MapRoute(
               "simpleApartmentsList",
               "BMBuilding/GetSimpleApartmentsList/{id}",
               new
               {
                   controller = "BMBuilding",
                   action = "GetSimpleApartmentsList"
               });

            RouteTable.Routes.MapRoute(
               "bmbuildingapartmentassignmentdetails",
               "BMBuilding/bmbuildingapartmentassignmentdetails",
               new
               {
                   controller = "BMBuilding",
                   action = "BMBuildingApartmentAssignmentDetails"
               });

            RouteTable.Routes.MapRoute(
                "BMBuildingApartmentStatuses",
                "BMBuilding/GetApartmentStatuses",
                new
                {
                    controller = "BMBuilding",
                    action = "GetApartmentStatuses"
                });          

            RouteTable.Routes.MapRoute(
                "UnAuthorizedAccess",
                "Error/401",
                new
                {
                    controller = "Error",
                    action = "UnAuthorizedAccess"
                });

            RouteTable.Routes.MapRoute(
                "ForbiddenAccess",
                "Error/403",
                new
                {
                    controller = "Error",
                    action = "ForbiddenAccess"
                });

            RouteTable.Routes.MapRoute(
                "PageNotFound",
                "Error/404",
                new
                {
                    controller = "Error",
                    action = "PageNotFound"
                });

            RouteTable.Routes.MapRoute(
                "InternalServerError",
                "Error/500",
                new
                {
                    controller = "Error",
                    action = "InternalServerError"
                });

            RouteTable.Routes.MapRoute(
                "ServiceUnavailable",
                "Error/503",
                new
                {
                    controller = "Error",
                    action = "ServiceUnavailable"
                });

            RouteTable.Routes.MapRoute(
                "GatewayTimeout",
                "Error/504",
                new
                {
                    controller = "Error",
                    action = "GatewayTimeout"
                });

            RouteTable.Routes.MapRoute(             
               "BMBuildingGetSvgEmbedByMediaId",
               "BMBuilding/BMBuildingGetSvgEmbedByMediaId",
               new
               {
                   controller = "BMBuilding",
                   action = "BMBuildingGetSvgEmbedByMediaId"
               });

            RouteTable.Routes.MapRoute(
               "BMBuildingGetSvgByMediaId",
               "BMBuilding/BMBuildingGetSvgByMediaId",
               new
               {
                   controller = "BMBuilding",
                   action = "BMBuildingGetSvgByMediaId"
               });

            RouteTable.Routes.MapRoute(
               "BMBuildingSlider",
               "embed/{guid}",
               new
               {
                   controller = "BMBuildingSlider",
                   action = "BMEmbedBuildingSlider"
               });
        }
    }
}