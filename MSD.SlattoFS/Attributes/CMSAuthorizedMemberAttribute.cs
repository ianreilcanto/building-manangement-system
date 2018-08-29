using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Security;

namespace MSD.SlattoFS.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CMSAuthorizedMemberAttribute : AuthorizeAttribute
    {
        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if(httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            bool result = false;
            try
            {
                ApplicationContext appContext = ApplicationContext.Current;
                UmbracoContext umbContext = UmbracoContext.Current;
                if (!appContext.IsConfigured)
                {
                    result = false;
                }else
                {
                    //result = umbContext.Security.ValidateCurrentUser();
                    MembershipHelper membershipHelper = new MembershipHelper(umbContext);
                    result = membershipHelper.IsMemberAuthorized();
                }

            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/", false);
            filterContext.RequestContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
        }
    }
}