using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Repositories;
using Umbraco.Web;
using Umbraco.Web.Security;
using MSD.SlattoFS.Helpers;
using MSD.SlattoFS.Shared;
using System;
using Umbraco.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace MSD.SlattoFS.Services
{
    public class BuildingManager : ContentManager
    {
        private readonly IPocoRepository<Building> _buildingRepo;
        private readonly IPocoRepository<Address> _addressRepo;
        private readonly MembershipHelper _memberHelper;

        private readonly Dictionary<string, string> _buildingActionTemplates;

        public BuildingManager(MembershipHelper memberHelper, UmbracoContext context)
            : base(context)
        {
            _buildingRepo = new BuildingRepository();
            _addressRepo = new AddressRepository();

            _memberHelper = memberHelper;
            _buildingActionTemplates = new Dictionary<string, string>();
            _buildingActionTemplates.Add("Configure", "BMBuildingConfiguration");//actionName,templateAlias
            _buildingActionTemplates.Add("Edit", "BMBuildingEdit");
        }

        public Building CreateBuilding(Building building, string documentType, int parentId = 0)
        {
            //TODO: this way to find the 'main' parent sucks, should find a better way
            //through umbraco application published context
            var accountPropValue = Context.PublishedContentRequest.PublishedContent
                                    .Parent
                                    .Parent
                                    .GetValidPropertyValue(Constants.ACCOUNT_PROPERTY_ALIAS).ToString();

            int accountId = -1;
            int.TryParse(accountPropValue, out accountId);

            if (accountId <= 0) return null;

            //if account prop value was found
            var memberId = _memberHelper.GetCurrentMember().Id;

            Building newBuilding = new Building();
            newBuilding.AccountId = accountId;
            newBuilding.CreatedBy = memberId;
            newBuilding.ModifiedBy = memberId;
            newBuilding.CreatedOn = DateTime.Now;
            newBuilding.ModifiedOn = DateTime.Now;
            newBuilding.Description = building.Description;
            newBuilding.Name = building.Name;


            var buildingInserted = _buildingRepo.Insert(newBuilding);

            //Try Create Building Content on backoffice
            if (buildingInserted != null)
            {
                IContent buildingContent = CreateBuildingContent(buildingInserted, documentType, parentId);
            }

            return buildingInserted;
        }

        public IContent CreateBuildingContent(Building building, string documentType, int parentId = 0)
        {
            IContent buildingContent = InsertContent(building.Name, documentType, parentId);
            if (buildingContent != null)
            {
                SetBldgPropertyValue(buildingContent, building.Id);
                CreateBuildingActionItems(buildingContent, building.Id);
            }
            return buildingContent;
        }

        /// <summary>
        /// Method to create the action items under a building node type
        /// for this version, "Configure" and "Edit" are needed
        /// </summary>
        /// <param name="content"></param>
        /// <param name="actionItems"></param>
        /// <returns></returns>
        public bool CreateBuildingActionItems(IContent content, int buildingId, List<string> actionItems = null)
        {
            if (actionItems == null || actionItems.Count == 0)
            {
                actionItems = new List<string>(_buildingActionTemplates.Keys);
            }

            try
            {
                List<string> actionItemsAdded = new List<string>();
                foreach (var actionItem in actionItems.Select(a => a.ToString()))
                {
                    if (!actionItemsAdded.Contains(actionItem))
                    {
                        var actionItemContent = base.InsertContent(actionItem, Constants.BUILDING_DOCUMENTTYPE_ALIAS, content.Id);
                        //set building id property
                        SetBldgPropertyValue(actionItemContent, buildingId);
                        //set the isActionItem property enabled automatically
                        if (actionItemContent != null)
                        {
                            var actionItemProp = actionItemContent.Properties[Constants.ACTION_ITEM_ALIAS];
                            if (actionItemProp != null)
                            {
                                actionItemContent.SetValue(Constants.ACTION_ITEM_ALIAS, true);
                            }
                        }
                        //set Template
                        SetTemplate(actionItemContent, actionItem);

                        SaveChangesToContent(actionItemContent);

                        actionItemsAdded.Add(actionItem);
                    }
                }

                return actionItemsAdded.Count == actionItems.Count;
            }
            catch (Exception ex)
            {
                //TODO: log error
                return false;
            }
        }

        private IContent SetBldgPropertyValue(IContent content, int value)
        {
            var buildPropExist = content.Properties[Constants.BUILDING_PROPERTY_ALIAS];
            if (buildPropExist != null)
            {
                content.SetValue(Constants.BUILDING_PROPERTY_ALIAS, value);
                SaveChangesToContent(content);
                return content;
            }
            else
            {
                return null;
            }
        }

        private IContent SetTemplate(IContent content, string actionItem)
        {
            var templateAlias = _buildingActionTemplates[actionItem];
            var templates = content.ContentType.AllowedTemplates;
            foreach (var template in templates)
            {
                if (template.Alias == templateAlias)
                {
                    content.Template = template;
                }
            }
            return content;
        }

    }
}