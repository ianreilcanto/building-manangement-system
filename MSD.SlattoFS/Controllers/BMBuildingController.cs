using MSD.SlattoFS.Attributes;
using MSD.SlattoFS.Controllers.Base;
using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models;
using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Repositories;
using MSD.SlattoFS.Services;
using System;
using System.Web.Mvc;
using umbraco.cms.businesslogic.web;
using Umbraco.Web.Models;
using MSD.SlattoFS.Shared;
using System.Collections.Generic;
using MSD.SlattoFS.Models.ViewModels;
using System.Linq;
using Umbraco.Web;
using MSD.SlattoFS.Helpers;
using System.IO;
using Umbraco.Core.Logging;
using MSD.SlattoFS.Interface;
using MSD.SlattoFS.Models.Pocos.Base;
using Umbraco.Core.Services;
using System.Text;

namespace MSD.SlattoFS.Controllers
{
    public class BMBuildingController : SurfaceRenderMvcController
    {
        private readonly IPocoRepository<Building> _buildingRepo;
        private readonly IPocoRepository<Account> _accountRepo;
        private AddressManager _addressManager;
        private BuildingManager _buildingManager;
        private ApartmentRepository _apartmentRepo;
        private MediaManager _mediaManager;
        private AssetManager _assetManager;
        private ApartmentManager _apartmentManager;
        ISVGDataSource _svgDatasource;

        private const string MEDIAUPDATE_KEY = "mediaUpdate";
        private const string BUILDINGID_KEY = "buildingId";

        public BMBuildingController()
        {
            _buildingRepo = new BuildingRepository();
            _accountRepo = new AccountRepository();
            _apartmentRepo = new ApartmentRepository();
            _svgDatasource = new SVGDataSource();
        }

        public MediaManager MediaManager
        {
            get
            {
                if (_mediaManager == null)
                    _mediaManager = new MediaManager(UmbracoContext);
                return _mediaManager;
            }
        }
        public AssetManager AssetManager
        {
            get
            {
                if (_assetManager == null)
                    _assetManager = new AssetManager();
                return _assetManager;
            }
        }
        public ApartmentManager ApartmentManager
        {
            get
            {
                if (_apartmentManager == null)
                    _apartmentManager = new ApartmentManager(MediaManager);
                return _apartmentManager;
            }
        }
        public AddressManager AddressManager
        {
            get
            {
                if (_addressManager == null)
                    _addressManager = new AddressManager();
                return _addressManager;
            }
        }
        private BuildingManager BuildingManager
        {
            get
            {
                if (_buildingManager == null)
                    _buildingManager = new BuildingManager(Members, UmbracoContext.Current);
                return _buildingManager;
            }
        }


        [CMSAuthorizedMember]
        public ActionResult BMBuildingPage(RenderModel model)
        {
            if (model == null || model.Content == null)
            {
                return CurrentUmbracoPage();
            }

            if (model.Content.IsPropertyValid(Constants.BUILDING_PROPERTY_ALIAS))
            {
                int bldgId = -1;
                int.TryParse(model.Content.GetValidPropertyValue(Constants.BUILDING_PROPERTY_ALIAS).ToString(), out bldgId);

                if (bldgId > -1)
                {
                    var bldg = _buildingRepo.GetById(bldgId);
                    var assetManager = new AssetManager();

                    if (bldg != null)
                    {
                        var buildingModel = new BuildingViewModel(CurrentPage);
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
                        var mediaIds = new List<int>();
                        foreach (var media in assets)
                        {
                            mediaIds.Add(media.MediaId);
                        }
                        buildingModel.MediaItems = mediaIds;
                        buildingModel.Apartments = _apartmentRepo.GetAllByBuildingId(bldg.Id);

                       // var pdfStatus = new ApartmentAssetPdfViewModel();

                        foreach (var apartment in buildingModel.Apartments)
                        {
                            var apartmenPdf = assetManager.GetByApartmentId(apartment.Id);

                            var pdfStatus = new ApartmentAssetPdfViewModel();

                            if (apartmenPdf != null)
                            {
                                pdfStatus.HasPdf = true;
                                pdfStatus.ApartmentId = apartment.Id;
                               
                            }
                            else {

                                pdfStatus.HasPdf = false;
                                pdfStatus.ApartmentId = apartment.Id;
                            }

                            buildingModel.PdfStatus.Add(pdfStatus);                                                 
                       }

                        return CurrentTemplate(buildingModel);
                    }
                }

                return CurrentUmbracoPage();
            }
            return CurrentUmbracoPage();
        }

        /// <summary>
        /// This is action called via Template route hijacking, template
        /// has to be registered under the Templates tab on a document type in BMBuilding
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [CMSAuthorizedMember]
        public ActionResult BMBuildingCreate(RenderModel model)
        {
            return CurrentTemplate(model);
        }

        [CMSAuthorizedMember]
        public ActionResult BMBuildingEdit(RenderModel model)
        {
            var buildingModel = new BuildingViewModel(CurrentPage);
            if (model == null || model.Content == null)
            {
                return CurrentTemplate(buildingModel);
            }

            int bldgId = -1;
            if (model.Content.IsPropertyValid(Constants.BUILDING_PROPERTY_ALIAS))
            {
                int.TryParse(model.Content.GetValidPropertyValue(Constants.BUILDING_PROPERTY_ALIAS).ToString(), out bldgId);
            }
            else
            {
                int.TryParse(model.Content.Parent.GetValidPropertyValue(Constants.BUILDING_PROPERTY_ALIAS).ToString(), out bldgId);
            }

            if (bldgId > -1)
            {
                var bldg = _buildingRepo.GetById(bldgId);
                if (bldg != null)
                {
                    buildingModel.Id = bldg.Id;
                    buildingModel.AccountId = bldg.AccountId;
                    buildingModel.Name = bldg.Name;
                    buildingModel.Description = bldg.Description;
                    buildingModel.CreatedOn = bldg.CreatedOn;
                    buildingModel.CreatedBy = bldg.CreatedBy;
                    buildingModel.ModifiedBy = bldg.ModifiedBy;
                    buildingModel.ModifiedOn = bldg.ModifiedOn;
                    buildingModel.Addresses = AddressManager.GetByBuildingId(bldg.Id);

                    buildingModel.Guid = bldg.Guid;

                    return PartialView("~/Views/BMBuildingEdit.cshtml", buildingModel);
                }
            }
            return CurrentTemplate(buildingModel);
        }

        [CMSAuthorizedMember]
        public ActionResult BMBuildingConfiguration(RenderModel model)
        {
            var configVM = new BuildingConfigViewModel(model.Content);

            int bldgId = -1;
            if (model.Content.IsPropertyValid(Constants.BUILDING_PROPERTY_ALIAS))
            {
                int.TryParse(model.Content.GetValidPropertyValue(Constants.BUILDING_PROPERTY_ALIAS).ToString(), out bldgId);
            }
            else
            {
                int.TryParse(model.Content.Parent.GetValidPropertyValue(Constants.BUILDING_PROPERTY_ALIAS).ToString(), out bldgId);
            }

            if (bldgId > 0)
            {
                var building = _buildingRepo.GetById(bldgId);
                if (building != null)
                {
                    configVM.BuildingId = building.Id;
                    configVM.AccountId = building.AccountId;
                }
            }

            return CurrentTemplate(configVM);
        }

        /// <summary>
        /// Method called async to upload excel files, 
        /// pdf files for a building or an apartment
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BMBuildingUploadInfo()
        {
            //guard clause
            if (Request.Files == null || Request.Files.Count == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            //check if request has valid keys
            var requestResult = BuildingRequestHelper.GetRequestResult(Request);
            if (!requestResult.IsValid)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            //identify type of file uploaded ( excel or csv)
            bool success = false;
            var message = "There was a problem with the upload. Please contact administrator.";
            List<ApartmentViewModel> list = null;
            if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
            {
                var uploadedFile = Request.Files[0];
                var extension = Path.GetExtension(uploadedFile.FileName);
                try
                {
                    //TODO: the static factory causes some issue with repo dependencies
                    //var dataSourceManager = DataSourceServiceFactory.GetService(extension);
                    var building = _buildingRepo.GetById(requestResult.BuildingId);
                    if (building == null)
                    {
                        throw new ArgumentNullException("building");
                    }

                    var apartmentData = ApartmentManager.MapDataSourceToApartment(uploadedFile, requestResult.BuildingId) as List<Apartment>;
                    if (apartmentData != null && apartmentData.Count > 0)
                    {
                        list = apartmentData.Select(apt =>
                        {
                            var vm = ApartmentViewModel.CreateModel(apt);
                            return vm;

                        }).ToList();

                        success = true;
                        message = string.Concat("Successully uploaded ", uploadedFile.FileName);
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.Error<BMBuildingController>(ex.Message, ex);
                    success = false;
                }
            }

            return Json(new { success, message, list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method is called async from the front end
        /// This endpoint is registered on the Custom Route configuration object
        /// to ensure this is accessible
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BMBuildingApartmentUploadInfo()
        {
            //check if request has valid keys
            bool success = false;
            var requestResult = BuildingRequestHelper.GetAptRequestResult(Request);
            if (!requestResult.IsValid)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            //check if apartment does exist
            List<string> apartmentIdentifiers = new List<string>();
            IList<ApartmentAssetViewModel> list = null;
            var apartment = _apartmentRepo.GetById(requestResult.ApartmentId);
            if (apartment != null)
            {
                apartmentIdentifiers.Add(apartment.Name);

                var account = _accountRepo.GetById(requestResult.AccountId);
                var building = _buildingRepo.GetById(requestResult.BuildingId);

                if (account != null && building != null)
                {
                    var result = MediaManager.CreateFolder(account.Name, account.Id, building.Name, building.Id, apartmentIdentifiers);
                    if (result.MediaApartmentFolderIds != null && result.MediaApartmentFolderIds.Count > 0)
                    {
                        var aptMediaFolderId = result.MediaApartmentFolderIds.FirstOrDefault();
                        var mediaList = MediaManager.UploadMediaFiles(Request.Files, building.Id, aptMediaFolderId, apartment.Id);
                        if (mediaList != null && mediaList.Count > 0)
                        {
                            //identify the file assets for the apartment
                            var assets = MediaManager.GetApartmentMediaAssets(apartment.Id);
                            if (assets != null && assets.Count > 0)
                            {
                                list = assets.Select(x =>
                                {
                                    return new ApartmentAssetViewModel { MediaId = x.Id, Url = Umbraco.TypedMedia(x.Id).Url };
                                }).ToList();

                                success = true;
                            }
                        }
                    }
                }
            }



            return Json(new { success, list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get the building information and apartment's individual status etc
        /// This method is called async from front-end
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BMBuildingInfo()
        {
            //check if request has valid keys
            var requestResult = BuildingRequestHelper.GetRequestResult(Request);
            if (!requestResult.IsValid)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            bool success = false;

            //get the apartments registered on db for the building
            var apartments = ApartmentManager.GetApartments(requestResult.BuildingId);
            List<ApartmentViewModel> list = null;
            if (apartments != null && apartments.Count > 0)
            {
                list = apartments.Select(apt =>
                {
                    var vm = ApartmentViewModel.CreateModel(apt);

                    //identify the file assets for the apartment
                    //var apartment = _apartmentRepo.GetById(apt.Id);
                    var assets = MediaManager.GetApartmentMediaAssets(apt.Id);
                    if (assets != null && assets.Count > 0)
                    {
                        vm.FileAssets = assets.Select(x =>
                        {
                            return new ApartmentAssetViewModel { MediaId = x.Id, Url = Umbraco.TypedMedia(x.Id).Url };
                        }).ToList<ApartmentAssetViewModel>();
                    }
                    return vm;

                }).ToList();

                success = true;
            }

            return Json(new { success, list }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSimpleApartmentsList(int id)
        {
            var list = new List<BaseApartmentViewModel>();

            list.AddRange(ApartmentManager.GetSimpleApartmentsList(id).Select(x => new BaseApartmentViewModel()
            {
                Id = x.Id,
                Room = x.Room
            }));

            return Json(new { Message = "Sucess", list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method for uploading assets via client side async call
        /// NOTE that the param name is equal to the 'name' attribute on the
        /// input type 'file' name attribute
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BMBuildingUploadAssets()
        {
            //guard clause
            if (Request.Files == null || Request.Files.Count == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            //check if request has valid keys
            var requestResult = BuildingRequestHelper.GetRequestResult(Request);
            if (!requestResult.IsValid)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            IList<MediaViewModel> uploadedMediaList = null;

            var account = _accountRepo.GetById(requestResult.AccountId);
            var building = _buildingRepo.GetById(requestResult.BuildingId);
            if (account != null && building != null)
            {
                var result = MediaManager.CreateFolder(account.Name, account.Id, building.Name, building.Id);
                if (result.MediaBuildingFolderId > 0)
                {
                    var mediaList = MediaManager.UploadMediaFiles(Request.Files, building.Id, result.MediaBuildingFolderId);
                    if (mediaList != null && mediaList.Count > 0)
                    {
                        uploadedMediaList = mediaList.Select(x =>
                        {
                            var url = Umbraco.TypedMedia(x.Id) != null ? Umbraco.TypedMedia(x.Id).Url : "";
                            return MediaViewModel.CreateModel(x, url);

                        }).ToList();
                    }
                }
            }

            return new JsonResult
            {
                Data = new
                {
                    hasData = uploadedMediaList.Count > 0,
                    list = uploadedMediaList
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// Method called async to sort the assets of a building
        /// via the configuration > media tab
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BMBuildingSortAssets()
        {
            //guard clause
            var requestResult = BuildingRequestHelper.GetRequestResult(Request);
            if (!requestResult.IsValid)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            //guard close
            if (Request.Form[MEDIAUPDATE_KEY] == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            //update the sorting order
            var mediaList = Request.Form[MEDIAUPDATE_KEY].ToString().Split(new char[] { ',' });
            foreach (var mediaItem in mediaList)
            {
                var mediaInfoItem = mediaItem.Split(new char[] { '|' });
                var mediaId = mediaInfoItem[0];
                var mediaSortOrder = mediaInfoItem[1];
                var mediaName = mediaInfoItem[2];

                var media = MediaManager.GetMediaAsset(int.Parse(mediaId));
                if (media != null)
                {
                    //update the sort order property
                    media.SortOrder = int.Parse(mediaSortOrder);
                    media.Name = mediaName;
                    MediaManager.UpdateMediaAsset(media);
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult BMBuildingRemoveAsset(int id)
        {
            var isDeleted = MediaManager.DeleteMedia(id);
            AssetManager.DeleteByMediaId(id);
            ((SVGDataSource)_svgDatasource).DeleteByAssetId(id);
            
            return Json(new { success = isDeleted }, JsonRequestBehavior.AllowGet);
        }

        //map route to custom configuration handler
        [HttpPost]
        public ActionResult BMBuildingAssets()
        {
            var requestResult = BuildingRequestHelper.GetRequestResult(Request);
            if (!requestResult.IsValid)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            //find building from custom table;
            var building = _buildingRepo.GetById(requestResult.BuildingId);
            IList<MediaViewModel> uploadedMediaList = null;

            if (building != null && building.AccountId == requestResult.AccountId)
            {
                var list = MediaManager.GetBuildingMediaAssets(building.Id);
                list = list.OrderBy(i => i.SortOrder).ToList();
                if (list != null && list.Count > 0)
                {
                    uploadedMediaList = list.Select(x =>
                    {
                        var url = Umbraco.TypedMedia(x.Id) != null ? Umbraco.TypedMedia(x.Id).Url : "";
                        return MediaViewModel.CreateModel(x, url);
                    }).ToList();
                }
            }

            return new JsonResult
            {
                Data = new
                {
                    list = uploadedMediaList
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public JsonResult BMBuildingApartmentAssignmentDetails(int buildingId)
        {
            var apartmentAssignmentChecker = new ApartmentSvgAssignmentChecker();
            var apartmentAssignmentDetails = apartmentAssignmentChecker.GetApartmentAssignmentData(buildingId);

            return Json(new { apartmentAssignmentDetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetApartmentStatuses()
        {
            var apartmentStatusRepo = new ApartmentStatusRepository();
            return Json(new { apartmentStatuses = apartmentStatusRepo.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CMSAuthorizedMember]
        public ActionResult AddBuilding(Building building)
        {
            var newBuilding = BuildingManager.CreateBuilding(building, Constants.BUILDING_DOCUMENTTYPE_ALIAS, CurrentPage.Parent.Id);
            SaveNewAddresses(newBuilding.Id);
            return RedirectToUmbracoPage(CurrentPage.Parent);
        }

        [HttpPost]
        [CMSAuthorizedMember]
        public ActionResult UpdateBuilding(Building building)
        {
            if (building == null)
            {
                TempData["StatusMessage"] = "There was a problem with your request";
                return CurrentUmbracoPage();
            }
            building.ModifiedBy = Members.GetCurrentMember().Id;
            building.ModifiedOn = DateTime.Now;

            _buildingRepo.Update(building.Id, building);

            //updating existing addresses
            int addressCount = Int32.Parse(Request.Form["AddressCount"]);
            for (int i = 1; i < addressCount; i++)
            {
                int id = Int32.Parse(Request.Form["Address" + i.ToString()]);
                UpdateAddress(id, i, building.Id);
            }

            //adding new addresses
            SaveNewAddresses(building.Id);

            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult DeleteBuilding(Building building)
        {
            var bldgRepo = new BuildingRepository();
            bool isDeleted = bldgRepo.DeleteById(building.Id);
            //Delete addresses
            AddressManager.DeleteByBuildingId(building.Id);

            // Deletes the content.
            Document doc = new Document(CurrentPage.Parent.Id);
            doc.delete();

            return RedirectToUmbracoPage(CurrentPage.Parent.Parent);
        }

        [HttpPost]
        public ActionResult DeleteAddress(int addressId, int buildingId)
        {
            try
            {
                var deleted = AddressManager.DeleteBuildingAdress(addressId, buildingId);
                if (deleted)
                {
                    return Json(new { message = "Deleted", addressId = addressId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { message = "A building should have atleast one address.", addressId = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { message = "Delete error", addressId = addressId }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public ActionResult DownloadApartmentPDF(int apartmentId)
        {
            IPocoRepository<ApartmentAsset> assetRepo = new ApartmentAssetRepository();

            var assets = assetRepo.GetAllById(apartmentId);
            var mediaId = -1;
            if (assets.Count > 0)
            { mediaId = assets.FirstOrDefault().MediaId; }

            IMediaService mediaService = ApplicationContext.Services.MediaService;
            try
            {
                string pdfFromMedia = mediaService.GetById(mediaId).GetValue("umbracoFile").ToString();
                return Json(new { message = "Media: " + pdfFromMedia, pdf = pdfFromMedia }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { message = "There is no file for this", pdf = (string)null }, JsonRequestBehavior.AllowGet);
            }
        }

        private object UpdateAddress(int id, int addressIndex, int buildingId)
        {
            Address address = GetAddressForm(addressIndex);
            address.BuildingId = buildingId;
            if (address != null)
            {
                return AddressManager.UpdateAddress(id, address);
            }
            else
            {
                return null;
            }
        }

        private Address GetAddressForm(int addressIndex)
        {
            Address address = new Address();
            int addressId = 0;
            int.TryParse(Request.Form["Address" + addressIndex.ToString()], out addressId);
            if (addressId > 0)
            {
                address.Id = addressId;
                address.Address1 = Request.Form["Address" + addressIndex.ToString() + "_Address1"];
                address.Address2 = Request.Form["Address" + addressIndex.ToString() + "_Address2"];
                address.City = Request.Form["Address" + addressIndex.ToString() + "_City"];
                address.State_Province = Request.Form["Address" + addressIndex.ToString() + "_State"];
                address.Country = Request.Form["Address" + addressIndex.ToString() + "_Country"];
                address.ZipCode = Request.Form["Address" + addressIndex.ToString() + "_ZipCode"];
                return address;
            }
            else
            {
                return null;
            }
        }

        private int SaveNewAddresses(int buildingId)
        {
            if (Request.Form["newAddressLine"] != null)
            {

                var newAddress1s = Request.Form["newAddress1"].ToString().Split(new char[] { ',' });
                string[] address1Values = Request.Form.GetValues("newAddress1");
                string[] address2Values = Request.Form.GetValues("newAddress2");
                string[] cityValues = Request.Form.GetValues("newCity");
                string[] stateValues = Request.Form.GetValues("newState");
                string[] countryValues = Request.Form.GetValues("newCountry");
                string[] zipcodeValues = Request.Form.GetValues("newZipCode");
                int index = 0;
                while (index < address1Values.Length)
                {
                    //saveaddress
                    Address address = new Address();
                    address.BuildingId = buildingId;
                    address.Address1 = address1Values[index];
                    address.Address2 = address2Values[index];
                    address.City = cityValues[index];
                    address.State_Province = stateValues[index];
                    address.Country = countryValues[index];
                    address.ZipCode = zipcodeValues[index];
                    int addressId = AddressManager.SaveAddress(address);
                    index++;
                }
                return index;
            }
            else
            {
                return 0;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveSvgData(BuildingSvgConfigModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Svg))
            {
                model.Svg = string.Empty;
            }
                     
            var prevSvg = ApartmentManager.GetSvgData(model.BuildingId, model.AssetId);

            if (prevSvg != null)
            {
                prevSvg.Svg = model.Svg;
                ApartmentManager.UpdateSvgData(prevSvg);
            }
            else
            {
                var newBuildingSvg = new SvgData();
                newBuildingSvg.BuildingId = model.BuildingId;
                newBuildingSvg.AssetId = model.AssetId;
                newBuildingSvg.Svg = model.Svg;
                newBuildingSvg.Type = 0;

                ApartmentManager.SaveSvgData(newBuildingSvg);
            }

            return Json(new { Message = "Success" });
        }

        [HttpPost]
        public JsonResult GetSvgData(BuildingSvgConfigModel model)
        {
            var data = ApartmentManager.GetSvgData(model.BuildingId, model.AssetId);

            return Json(new { svgData = data });
        }

        [HttpPost]
        public JsonResult BMBuildingGetSvgByMediaId(int mediaId, int buildingId)
        {
            StringBuilder body = new StringBuilder(System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Views/Templates/SvgTemplate.html")));
            {
                body = body.Replace("{SvgData}", _svgDatasource.GetSVGData(buildingId, mediaId));
            }

            return Json(new { svgData = body.ToString() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BMBuildingGetSvgEmbedByMediaId(int mediaId, int buildingId)
        {
           
            StringBuilder body = new StringBuilder(System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Views/Templates/SvgEmbedTemplate.html")));
            {
                body = body.Replace("{SvgData}", _svgDatasource.GetSVGData(buildingId, mediaId));
          
            }

            return Json(new { svgData = body.ToString() }, JsonRequestBehavior.AllowGet);
        }
    }
}