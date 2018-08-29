using MSD.SlattoFS.Controllers.Base;
using System;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;
using MSD.SlattoFS.Models.ViewModels;
using MSD.SlattoFS.Services;
using MSD.SlattoFS.Attributes;
using MSD.SlattoFS.Helpers;
using MSD.SlattoFS.Shared;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using MSD.SlattoFS.Models.Pocos.Base;
using Umbraco.Core.Models;

namespace MSD.SlattoFS.Controllers
{
    public class BMBuildingListingController : SurfaceRenderMvcController
    {
        private readonly BuildingManagerService _buildingService;

        //lazy load
        private MediaManager _uploadManager;
        public MediaManager MediaManager
        {
            get
            {
                if (_uploadManager == null)
                    _uploadManager = new MediaManager(UmbracoContext);
                return _uploadManager;
            }
        }

        public BMBuildingListingController()
        {
            _buildingService = new BuildingManagerService();
        }

        [CMSAuthorizedMember]
        public ActionResult BMBuildingsPage(RenderModel model)
        {
            BuildingsViewModel buildingsViewModel = new BuildingsViewModel(model.Content);

            var buildings = CurrentPage.Children
                .Where(x => x.DocumentTypeAlias.Equals(Constants.BUILDING_DOCUMENTTYPE_ALIAS, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.IsPropertyValid(Constants.ACTION_ITEM_ALIAS))
                .Where(x => !Boolean.Parse(x.GetValidPropertyValue(Constants.ACTION_ITEM_ALIAS).ToString()));

            if (buildings != null)
            {
                //get building with correctly set building property
                foreach (var bldg in buildings.Where(b => b.IsPropertyValid(Constants.BUILDING_PROPERTY_ALIAS)))
                {
                    try
                    {
                        int buildingId = -1;
                        int.TryParse(bldg.GetValidPropertyValue(Constants.BUILDING_PROPERTY_ALIAS).ToString(), out buildingId);
                        if (buildingId > -1)
                        {
                            //check if actual building object from db exists
                            var building = _buildingService.GetBuildingById(buildingId);
                            if (building != null)
                            {
                                //TODO: or better get from property 'description' in content node
                                var buildingInfo = BuildingInformation.CreateModel(
                                        building.Id,
                                        building.Name,
                                        building.Description
                                    );
                                buildingInfo.Name = building.Name;
                                buildingInfo.Description = building.Description; //TODO: or better get from property in content node

                                IAsset defaultMediaAsset = MediaManager.GetDefaultBuildingAsset(buildingId);
                                if (defaultMediaAsset != null) buildingInfo.SetDefaultAsset(defaultMediaAsset);


                                //TODO: wrap this on an address manager
                                var addresses = _buildingService.GetAddresses(building);
                                if (addresses != null && addresses.Count > 0)
                                {
                                    buildingInfo.Addresses = addresses;
                                }
                                //================================

                                buildingsViewModel.Buildings.Add(buildingInfo);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error<BMBuildingListingController>(ex.Message, ex);
                        continue;
                    }
                }
            }

            return CurrentTemplate(buildingsViewModel);
        }
    }
}