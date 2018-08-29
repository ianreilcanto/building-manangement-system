using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MSD.SlattoFS.Services
{
    public class BuildingManagerService
    {
        private readonly IPocoRepository<Building> _buildingRepo;
        private readonly IPocoRepository<BuildingAsset> _buildingAssetRepo;
        private readonly IPocoRepository<Address> _addressRepo;
        public BuildingManagerService()
        {
            _buildingRepo = new BuildingRepository();
            _buildingAssetRepo = new BuildingAssetRepository();
            _addressRepo = new AddressRepository();
        }

        public Building GetBuildingById(int buildingId)
        {
            return _buildingRepo.GetById(buildingId);
        }

        //get all images on a building
        public IList<BuildingAsset> GetBuildingAssets(int buildingId)
        {
            return _buildingAssetRepo.GetAllById(buildingId);
        }

        /// <summary>
        /// Get specific building asset based on its id and building
        /// Not specifying the asset id will default to first asset object in 
        /// a building's asset collection
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public BuildingAsset GetBuildingAsset(int buildingId, int assetId = -1)
        {
            IList<BuildingAsset> buildingAssets = GetBuildingAssets(buildingId);
            if (buildingAssets != null && buildingAssets.Count > 0)
            {
                if (assetId > 0)
                    return _buildingAssetRepo.GetById(assetId);
                return buildingAssets.FirstOrDefault();
            }

            return null;
        }

        // get 1 image for List View
        public BuildingAsset GetById(int buildingId)
        {
            BuildingAsset existingAsset = null;
            var buildingAssets = _buildingAssetRepo.GetAllById(buildingId);
            if (buildingAssets != null && buildingAssets.Count > 0)
            {
                existingAsset = buildingAssets.FirstOrDefault();
            }

            return existingAsset;    
        }

        public List<Address> GetAddresses(Building building)
        {
            List<Address> addresses = new List<Address>();
            AddressRepository addressRepo = new AddressRepository();
            addresses = addressRepo.GetAllByBuildingId(building.Id);
            return addresses;
        }

        public Address GetAddressesById(int addressId)
        {
            return ((AddressRepository)_addressRepo).GetById(addressId);
        }

    }
}
