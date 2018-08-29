using MSD.SlattoFS.Controllers.Base;
using MSD.SlattoFS.Helpers;
using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models;
using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Models.ViewModels;
using MSD.SlattoFS.Repositories;
using MSD.SlattoFS.Services;
using MSD.SlattoFS.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace MSD.SlattoFS.Controllers
{
    public class BMBuildingSliderController : SurfaceRenderMvcController
    {
        private readonly IPocoRepository<Building> _buildingRepo;
        private readonly IPocoRepository<Account> _accountRepo;
        private ApartmentRepository _apartmentRepo;

        private AssetManager _assetManager;
        public AssetManager AssetManager
        {
            get
            {
                if (_assetManager == null)
                    _assetManager = new AssetManager();
                return _assetManager;
            }
        }


        public BMBuildingSliderController()
        {
            _buildingRepo = new BuildingRepository();
            _accountRepo = new AccountRepository();
            _apartmentRepo = new ApartmentRepository();
        }

        public ActionResult BMEmbedBuildingSlider(Guid guid)
        {

            var bldg = ((BuildingRepository)_buildingRepo).GetByGuid(guid);

            if (bldg != null)
            {
                var buildingModel = new SliderViewModel();
                buildingModel.Id = bldg.Id;
                buildingModel.AccountId = bldg.AccountId;
                buildingModel.Name = bldg.Name;
                buildingModel.Description = bldg.Description;
                buildingModel.CreatedOn = bldg.CreatedOn;
                buildingModel.CreatedBy = bldg.CreatedBy;
                buildingModel.ModifiedBy = bldg.ModifiedBy;
                buildingModel.ModifiedOn = bldg.ModifiedOn;

                buildingModel.AccountName = _accountRepo.GetById(bldg.AccountId).Name;

                var apartmentStatusRepo = new ApartmentStatusRepository();
                buildingModel.ApartmentStatuses = apartmentStatusRepo.GetAll() as List<ApartmentStatus>;

                buildingModel.Guid = bldg.Guid;

                var asset = new AssetManager();
                var assets = asset.GetAssets(bldg.Id);
                var mediaItems = new Dictionary<int, string>();
                foreach (var media in assets)
                {
                    var umbracoMedia = Umbraco.Media(media.MediaId);
                    mediaItems.Add(media.MediaId, umbracoMedia.Url);
                }
                buildingModel.MediaItems = mediaItems;
                buildingModel.Apartments = _apartmentRepo.GetAllByBuildingId(bldg.Id);
                
                return View("BMEmbedBuildingSlider", buildingModel);
            }
            return View();
        }
    }
}