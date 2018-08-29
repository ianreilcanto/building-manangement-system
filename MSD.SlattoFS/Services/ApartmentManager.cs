using MSD.SlattoFS.Interface;
using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Models.ViewModels;
using MSD.SlattoFS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Logging;
using Umbraco.Web;

namespace MSD.SlattoFS.Services
{
    public class ApartmentManager
    {
        private readonly IDataSourceService _dataSourceService;
        private readonly IPocoRepository<Apartment> _apartmentRepo;
        private readonly IPocoRepository<ApartmentAsset> _apartmentAssetRepo;
        private readonly IPocoRepository<Account> _accountRepo;
        private readonly IPocoRepository<Building> _buildingRepo;
        private readonly MediaManager _mediaManager;
        private readonly BuildingFolderRepository _buildingFolderRepo;
        private readonly ApartmentFolderRepository _apartmentFolderRepo;
        private readonly SVGDataRepository _svgRepository;

        public ApartmentManager()
        {
            _dataSourceService = new ExcelDataSourceService();
            _apartmentRepo = new ApartmentRepository();
            _apartmentAssetRepo = new ApartmentAssetRepository();
            _accountRepo = new AccountRepository();
            _buildingRepo = new BuildingRepository();

            _buildingFolderRepo = new BuildingFolderRepository();
            _apartmentFolderRepo = new ApartmentFolderRepository();
            _svgRepository = new SVGDataRepository();
        }

        public ApartmentManager(MediaManager mediaManager) : this()
        {
            _mediaManager = mediaManager;
        }
        
        /// <summary>
        /// Method that accepts an uploaded file and extracts mapped entities 
        /// from the datasource service
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Apartment> MapDataSourceToApartment(HttpPostedFileBase uploadedFile, object id)
        {
            try
            {
                var apartmentData = _dataSourceService.MapSourceToData(uploadedFile, id) as List<Apartment>;
                if(apartmentData != null && apartmentData.Count > 0)
                {
                    //try creating the folders needed in the media library if they have not bee created yet
                    TryCreatingMediaStructure(apartmentData, Convert.ToInt32(id));
                }
                return apartmentData;
            }
            catch (Exception ex)
            {
                LogHelper.Error<ApartmentManager>(ex.Message, ex);
                return null;
            }
        }

        public List<Apartment> GetApartments(int buildingId)
        {
            return _apartmentRepo.GetAll().Where(a=> a.BuildingId == buildingId).ToList();
        }

        public List<BaseApartmentViewModel> GetSimpleApartmentsList(int buildingId)
        {
            return GetApartments(buildingId).Select(x => new BaseApartmentViewModel() { Id = x.Id, Room = x.Name }).ToList();
        }

        private void TryCreatingMediaStructure(List<Apartment> apartments, int buildingId)
        {
            try
            {
                var building = _buildingRepo.GetById(buildingId);
                var account = _accountRepo.GetById(building.AccountId);
                var apartmentIdentifiers = apartments.Select(x => x.Name).ToList();
                var result = _mediaManager.CreateFolder(account.Name, account.Id, building.Name, building.Id, apartmentIdentifiers);
                
            }
            catch (Exception ex)
            {
                LogHelper.Error<ApartmentManager>(ex.Message, ex);
            }
        }

        public SvgData GetSvgData(int buildingId, int assetId)
        {
            return _svgRepository.GetByBuildingId(buildingId).Where(x => x.AssetId == assetId).FirstOrDefault();
        }

        public SvgData SaveSvgData(SvgData data)
        {
            return _svgRepository.Insert(data);
        }

        public bool UpdateSvgData(SvgData data)
        {
            return _svgRepository.Update(data.Id, data);
        }
    }
}