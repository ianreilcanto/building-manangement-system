using MSD.SlattoFS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace MSD.SlattoFS.Controllers
{
    public class ErrorController : SurfaceController
    {
        private readonly ErrorViewModel Error;
        public ErrorController()
        {
            Error = new ErrorViewModel();
        }

        public ActionResult BadRequest()
        {
            Error.CodeStatus = 400;
            Error.Description = "Bad Request";
            Error.Message = "Your browser sent a request that this server could not understand.";
            return View("ErrorPage", Error);
        }

        public ActionResult UnAuthorizedAccess()
        {
            Error.CodeStatus = 401;
            Error.Description = "Authorization Required";
            Error.Message = "You are not authorized to view this page due to invalid credentials.";
            return View("ErrorPage", Error);
        }

        public ActionResult ForbiddenAccess()
        {
            Error.CodeStatus = 403;
            Error.Description = "Forbidden";
            Error.Message = "Sorry! Access is denied.";
            return View("ErrorPage", Error);
        }

        public ActionResult PageNotFound()
        {
            Error.CodeStatus = 404;
            Error.Description = "Page Not Found";
            Error.Message = "We're sorry but the page you're looking for does not exist.";
            return View("ErrorPage", Error);
        }

        public ActionResult InternalServerError()
        {
            Error.CodeStatus = 500;
            Error.Description = "Internal Server Error";
            Error.Message = "An error occurred and your request couldn't be completed.";
            return View("ErrorPage", Error);
        }

        public ActionResult ServiceUnavailable()
        {
            Error.CodeStatus = 503;
            Error.Description = "Service Unavailable";
            Error.Message = "The service you requested is not available yet.";
            return View("ErrorPage", Error);
        }

        public ActionResult GatewayTimeout()
        {
            Error.CodeStatus = 504;
            Error.Description = "Gateway Timeout";
            Error.Message = "Something did not respond fast enough, that's all we know.";
            return View("ErrorPage", Error);
        }
    }
}
