using MSD.SlattoFS.Models.ViewModels;
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Umbraco.Core.Models;
using MSD.SlattoFS.Shared;
using MSD.SlattoFS.Models.Pocos;

namespace MSD.SlattoFS.Helpers
{
    public static class BuildingsHtmlHelper
    {
        public static MvcHtmlString BuildingsListing(this HtmlHelper helper,
            IPublishedContent contentModel,
            string listViewType = "thumbnails")
        {
            StringBuilder sb = new StringBuilder();

            //check if building is under the account
            //check if the account id matches the account/accountid property of the building publishedcontent
            if (!contentModel.Parent.DocumentTypeAlias.Equals(Constants.ACCOUNT_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
            {
                return new MvcHtmlString("<p>There is a problem with your request. Building is not found under account. Please check with administrator</p>");
            }

            //get the building nodes where doctype type/alias is of 'bmbuilding'
            //and NON-Action items
            var buildings = contentModel.Children
                .Where(x => x.DocumentTypeAlias.Equals(Constants.BUILDING_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.IsPropertyValid(Constants.ACTION_ITEM_ALIAS))
                .Where(x => !Boolean.Parse(x.GetValidPropertyValue(Constants.ACTION_ITEM_ALIAS).ToString()));

            if (buildings != null)
            {
                sb.Append("<ul class='" + listViewType + "'>");
                foreach (var building in buildings.Where(b => b.IsPropertyValid(Constants.BUILDING_PROPERTY_ALIAS)))
                {
                    var buildingId = building.GetValidPropertyValue(Constants.BUILDING_PROPERTY_ALIAS).ToString();
                    if (buildingId != null)
                    {
                        sb.Append(string.Concat("<li><a href='", building.Url, "'>", building.Name, "</a></li>"));
                    }

                }
                sb.Append("</ul>");
            }
            else
            {
                sb.Append("<p>No buildings found on account. Please check with administrator.</p>");
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString ThumbnailView(this HtmlHelper helper,
            BuildingsViewModel model,
            string listViewType = "thumbnails")
        {
            StringBuilder sb = new StringBuilder();

            //check if building is under the account
            //check if the account id matches the account/accountid property of the building publishedcontent
            if (!model.Content.Parent.DocumentTypeAlias.Equals(Constants.ACCOUNT_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
            {
                return new MvcHtmlString("<p>There is a problem with your request. Building is not found under account. Please check with administrator</p>");
            }

            var buildings = model.Content.Children
                .Where(x => x.DocumentTypeAlias.Equals(Constants.BUILDING_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.IsPropertyValid(Constants.ACTION_ITEM_ALIAS))
                .Where(x => !Boolean.Parse(x.GetValidPropertyValue(Constants.ACTION_ITEM_ALIAS).ToString()));

            if (buildings != null)
            {
                foreach (var building in buildings.Where(x => x.IsPropertyValid(Constants.BUILDING_PROPERTY_ALIAS)))
                {
                    int buildingId = -1;
                    int.TryParse(building.GetValidPropertyValue(Constants.BUILDING_PROPERTY_ALIAS).ToString(), out buildingId);
                    if (buildingId > -1)
                    {
                        var buildingDetail = model.Buildings
                            .Where(b => b.Id == buildingId)
                            .FirstOrDefault();

                        //find the details, if it exists
                        if (buildingDetail != null)
                        {
                            sb.Append(Details(buildingDetail, building.Url, building.Name));
                        }
                    }
                }
            }

            return new MvcHtmlString(sb.ToString());

        }

        private static MvcHtmlString Details(BuildingInformation building, string url, string name)
        {
            var imagePath = "";
            if (building.Assets == null || building.Assets.Count == 0)
            {
                imagePath = "/images/no_thumbnail.jpg"; //default
            }
            else
            {
                imagePath = building.Assets.FirstOrDefault().Url;
            }

            StringBuilder thumbnail = new StringBuilder();
            //thumbnail.Append("<div class='item col-lg-3 col-md-4 col-xs-6 thumb'>");
            thumbnail.Append("<div class='item col-xs-4 col-lg-4'>");
            thumbnail.Append(string.Concat("<div class='thumbnail'><a href='", url, "'>"));
            thumbnail.Append(string.Concat("<img class='img-responsive' src='", imagePath, "' alt=''>"));
            thumbnail.Append("</a>");

            thumbnail.Append(string.Concat("<div class='caption'><h4 class='group inner list-group-item-heading building-name'><a href='", url, "'>", name, "</a></h4></br>"));

            //get the addresses strings
            if (building.Addresses != null && building.Addresses.Count > 0)
            {
                var cnt = 1;
                foreach (var address in building.Addresses)
                {
                    if (address != null)
                    {
                        //thumbnail.Append(string.Concat("<label>Location ", cnt, ": ", address.City, " ", address.StreetNumber, "</label></br>"));
                        thumbnail.Append(string.Concat("<p class='group inner list-group-item-text'>"));
                        thumbnail.Append(string.Concat("<div class='building-description'></div>"));
                        thumbnail.Append(string.Concat("<label>Location ", cnt, ": ", address.City, " ", address.Address1, "</label></br>"));
                        cnt++;
                    }
                }
            }

            thumbnail.Append("</div></div></div>");
            return new MvcHtmlString(thumbnail.ToString());
        }

        /// <summary>
        /// get the children nodes for action item links of a bmbuilding type
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="contentModel"></param>
        /// <returns></returns>
        public static MvcHtmlString BuildingActionLinks(this HtmlHelper helper, IPublishedContent contentModel)
        {
            StringBuilder sb = new StringBuilder();

            //check if building is under the account
            if (!contentModel.Parent.DocumentTypeAlias.Equals(Constants.BUILDINGLISTING_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
            {
                return new MvcHtmlString("<p>There is a problem with your request. Action Item is not found under account. Please check with administrator</p>");
            }

            //get the nodes where doctype type/alias is of 'bmbuilding'
            var actions = contentModel.Children
                .Where(x => x.DocumentTypeAlias.Equals(Constants.BUILDING_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.IsPropertyValid(Constants.ACTION_ITEM_ALIAS))
                .Where(x => Boolean.Parse(x.GetValidPropertyValue(Constants.ACTION_ITEM_ALIAS).ToString()));

            if (actions != null)
            {
                sb.Append("<ul class='buildings-link'>");
                foreach (var action in actions)
                {
                    sb.Append(string.Concat("<a href='", action.Url, "'><li><span class='buildings-link-name " + action.Name + "'>", action.Name, "</li></a>"));
                }
                sb.Append("</ul>");
            }
            else
            {
                sb.Append("<p>No buildings found on account. Please check with administrator.</p>");
            }

            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// Get the children nodes of the buildings node where
        /// item is flagged/set as action items (e.g. create, others)
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="contentModel"></param>
        /// <returns></returns>
        public static MvcHtmlString BuildingsActionLinks(this HtmlHelper helper,
            IPublishedContent contentModel)
        {
            StringBuilder sb = new StringBuilder();

            //check if building is under the account
            //TODO: check if the account id matches the account/accountid property of the building publishedcontent
            if (!contentModel.Parent.DocumentTypeAlias.Equals(Constants.ACCOUNT_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
            {
                return new MvcHtmlString("<p>There is a problem with your request. Building is not found under account. Please check with administrator</p>");
            }

            //get the nodes where doctype type/alias is of 'bmbuilding'
            var actions = contentModel.Children
                .Where(x => x.DocumentTypeAlias.Equals(Constants.BUILDING_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.IsPropertyValid(Constants.ACTION_ITEM_ALIAS))
                .Where(x => Boolean.Parse(x.GetValidPropertyValue(Constants.ACTION_ITEM_ALIAS).ToString()));

            if (actions != null)
            {
                sb.Append("<ul class='building-link'>");
                foreach (var action in actions)
                {
                    sb.Append(string.Concat("<a href='", action.Url, "'><li><span class='building-link-name " + action.Name + "'>", action.Name, "</li></a>"));
                }
                sb.Append("</ul>");
            }
            else
            {
                sb.Append("<p>No buildings found on account. Please check with administrator.</p>");
            }

            return new MvcHtmlString(sb.ToString());
        }

    }
}