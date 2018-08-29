using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Security;
using MSD.SlattoFS.Shared;

namespace MSD.SlattoFS.Helpers
{
    public static class AccountHtmlHelper
    {
        private static string[] AccountPageLinks = { Constants.ACCOUNT_DOCUMENTTYPE_ALIAS, Constants.BUILDINGLISTING_DOCUMENTTYPE_ALIAS };

        /// <summary>
        /// Helper method to check if the links are available and registered on the backoffice
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="contentModel"></param>
        /// <returns></returns>
        public static MvcHtmlString AccountLinks(this HtmlHelper helper, IPublishedContent contentModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class='account-list'>");
            foreach (var accountLink in AccountPageLinks)
            {

                var firstLevelChildNode = FirstChildDocumentType(contentModel, accountLink);
                if (firstLevelChildNode != null)
                {
                    sb.Append(string.Concat("<a href='", firstLevelChildNode.Url, "'><li class='" + firstLevelChildNode.Name + "'><span class='account-link-name'>", firstLevelChildNode.Name, "</span></li></a>"));
                }
            }
            sb.Append("</ul>");
            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// Helper method to generate the access/options available for the logged member user
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="contentModel"></param>
        /// <returns></returns>
        public static MvcHtmlString AccountSettings(this HtmlHelper helper, MembershipHelper membershipHelper, IPublishedContent contentModel)
        {            

            if (membershipHelper == null)
            {
                return MvcHtmlString.Empty;
            }

            if (!membershipHelper.IsLoggedIn())
            {
                return MvcHtmlString.Empty;
            }

            StringBuilder sb = new StringBuilder();

            //try get configuration starting node id for the application
            int applicationRootId = -1;
            int.TryParse(ConfigurationManager.AppSettings["rootNodeId"], out applicationRootId);

            //if no starting root id specified, skip and return
            if(applicationRootId <= 0)
            {
                return MvcHtmlString.Empty;
            }
            
            IPublishedContent dashboardContent = null;

            //Get the current member logged in
            var memberLoggedIn = membershipHelper.GetCurrentMember();
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            //fail fast if non found
            if (memberLoggedIn == null || umbracoHelper == null)
            {
                return MvcHtmlString.Empty;
            }

            //check if member logged in is ASSOCIATED with an account
            //check if property accountid is set and a value is assigned to it, otherwise default to ""
            if (!memberLoggedIn.IsPropertyValid(Constants.ACCOUNT_PROPERTY_ALIAS))
            {
                return MvcHtmlString.Empty;
            }          
                       
            int accountId = -1;
            int.TryParse(memberLoggedIn.GetValidPropertyValue(Constants.ACCOUNT_PROPERTY_ALIAS).ToString(), out accountId);

            //fail fast if account id was not found on member logged in
            if (accountId <= 0)
            {
                return MvcHtmlString.Empty;
            }

            //Try to get the Content NODE for the application ('home')
            IPublishedContent mainContent = umbracoHelper.TypedContent(applicationRootId);
            if (mainContent != null)
            {
                bool isEnabled = false;
                if (contentModel.DocumentTypeAlias.Equals(Constants.BUILDINGLISTING_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase) ||
                    contentModel.DocumentTypeAlias.Equals(Constants.BUILDING_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
                {
                    isEnabled = true;
                }

                //find the content node for the account user
                //dashboardContent = mainContent.FirstChild();
                dashboardContent =  mainContent.Children()
                        .Where(x => x.IsDocumentType(Constants.ACCOUNT_DOCUMENTTYPE_ALIAS))
                        .FirstOrDefault(x=> x.IsPropertyValid(Constants.ACCOUNT_PROPERTY_ALIAS) && (int)x.GetValidPropertyValue(Constants.ACCOUNT_PROPERTY_ALIAS) == accountId);

                if (dashboardContent != null)
                {
                    sb.Append(string.Concat("<li class='", contentModel.Id == dashboardContent.Id || isEnabled ? "current_page_item" : null, "'>"));
                    sb.Append(string.Concat("<a href='", dashboardContent.Url, "'>Dashboard</a>"));
                    sb.Append("</li>");
                }

                
            }
            return new MvcHtmlString(sb.ToString());
        }

/// <summary>
/// get the first child node based on its alias if it exist under a parent node
/// </summary>
/// <param name="content"></param>
/// <param name="alias"></param>
/// <returns></returns>
private static IPublishedContent FirstChildDocumentType(IPublishedContent content, string alias)
{
    IPublishedContent firstChildContent = null;
    if (content != null && content.Children != null)
    {
        firstChildContent = content.Children
            .FirstOrDefault(x => x.DocumentTypeAlias.Equals(alias, StringComparison.OrdinalIgnoreCase));
    }

    return firstChildContent;
}
    }
}