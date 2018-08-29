using MSD.SlattoFS.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Security;
using MSD.SlattoFS.Shared;

namespace MSD.SlattoFS.Helpers
{
    public static class NavigationHelper
    {
        /// <summary>
        /// Helper method to generate the appropriate control on the navigation
        /// bar for logged or non-logged member
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString AccessNavigation(this HtmlHelper helper, MembershipHelper membershipHelper)
        {
            StringBuilder builder = new StringBuilder();
            if (membershipHelper == null)
            {
                return MvcHtmlString.Empty;
            }

            if (membershipHelper.IsLoggedIn())
            {
                builder.Append(helper.Partial("BMAccountSettings"));
            }
            else
            {
                builder.Append(helper.Partial("BMLogin"));
            }

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString Breadcrumbs(this HtmlHelper helper, IPublishedContent currentContent)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ol class='breadcrumb' id='breadcrumbs'>");

            //just home
            if (currentContent.Ancestors().Count() == 0)
            {
                //DO nothing for now
                //sb.Append(string.Concat("<li>/", content.Name, "</li>"));
            }
            else
            {
                foreach (var ancestor in currentContent.Ancestors().Reverse().Where(t => t.IsVisible()))
                {
                    var crumbTitle = BreadcrumbTitle(ancestor);
                    sb.Append(string.Concat("<li class='breadcrumb-item'><a href ='", ancestor.Url, "'>", crumbTitle, "</a></li>"));
                }

                //THis is the current published content object
                sb.Append(string.Concat("<li class='breadcrumb-item active'><strong>", BreadcrumbTitle(currentContent), "</strong></li>"));
            }
            sb.Append("</ol>");
            return new MvcHtmlString(sb.ToString());
        }

        private static string BreadcrumbTitle(IPublishedContent content)
        {
            var crumbTitle = content.Name;
            var documentTypeAlias = content.DocumentTypeAlias;
            
            if (documentTypeAlias.Equals(Constants.BUILDINGLISTING_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
            {
                crumbTitle = "Projects";
            }
           
            if (documentTypeAlias.Equals(Constants.HOME_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
            {
                crumbTitle = "Home";
            }

            //if (documentTypeAlias.Equals(Constants.BUILDING_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
            //{
            //    crumbTitle = "Project";
            //}
            //if (documentTypeAlias.Equals(Constants.ACCOUNT_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
            //{
            //    crumbTitle = "Account";
            //}
            //if (documentTypeAlias.Equals(Constants.PROFILE_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
            //{
            //    crumbTitle = "Profile";
            //}

            return crumbTitle;
        }
    }
}