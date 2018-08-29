using System;
using System.Web.Mvc;
using Umbraco.Core.Logging;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.Security;

namespace MSD.SlattoFS.Controllers.Base
{ 
    public class SurfaceRenderMvcController : SurfaceController, IRenderMvcController
    {
        protected MembershipHelper MembershipHelper { get { return this.Members; } }
        /// <summary>
        /// Check to make sure physical template file exist on disk
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        protected bool EnsurePhysicalViewExists(string template)
        {
            var result = ViewEngines.Engines.FindView(ControllerContext, template, null);
            if (result.View == null)
            {
                LogHelper.Warn<SurfaceRenderMvcController>("No physical template file was found for the template " + template);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns actionresult based on template name found in the route values and the model given
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        protected ActionResult CurrentTemplate<T>(T model)
        {
            var template = ControllerContext.RouteData.Values["action"].ToString();
            if (!EnsurePhysicalViewExists(template))
            {
                return HttpNotFound();
            }

            return View(template, model);
        }
        
        /// <summary>
        /// Default action to render front-end view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Index(RenderModel model)
        {
            return CurrentTemplate(model);
        }

        #region OVERRIDES

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            //Log the exception.
            LogHelper.Error<SurfaceRenderMvcController>(filterContext.Exception.Message, filterContext.Exception);

            //Show the view error.
            filterContext.Result = View("Error");
            filterContext.ExceptionHandled = true;
        } 
        #endregion


    }
}