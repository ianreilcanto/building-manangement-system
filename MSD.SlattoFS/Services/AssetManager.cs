using MSD.SlattoFS.Factories;
using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Models.Pocos.Base;
using MSD.SlattoFS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace MSD.SlattoFS.Services
{
    public class AssetManager
    {
        private readonly IPocoRepository<BuildingAsset> _buildingAssetRepo;
        private readonly IPocoRepository<ApartmentAsset> _apartmentAssetRepo;
        public AssetManager()
        {
            _buildingAssetRepo = new BuildingAssetRepository();
            _apartmentAssetRepo = new ApartmentAssetRepository();
        }

        public void UpdateAsset(int buildingId, int mediaId)
        {
            //check if child media node already exists in the db
            var dbassetExisting = _buildingAssetRepo.GetById(mediaId);
            if (dbassetExisting != null)
            {
                dbassetExisting.BuildingId = buildingId;
                dbassetExisting.MediaId = mediaId;
                dbassetExisting.TypeId = (int)AssetMediaType.Image;

                //just update to make sure it's referenced to correct buildign and media
                _buildingAssetRepo.Update(dbassetExisting.Id, dbassetExisting);
            }
            else
            {
                //re-add each media to db asset table
                _buildingAssetRepo.Insert((BuildingAsset)AssetFactory.CreateAsset(AssetType.Building, buildingId, mediaId, AssetMediaType.Image));
            }
        }

        public IList<BuildingAsset> GetAssets(int buildingId)
        {
            return _buildingAssetRepo.GetAllById(buildingId);
        }

        public bool DeleteByMediaId(int mediaId)
        {
            return ((BuildingAssetRepository)_buildingAssetRepo).DeleteByMediaId(mediaId);
        }

        public ApartmentAsset GetByApartmentId(int id)
        {
            var apartmentAsset = _apartmentAssetRepo.GetAllById(id).FirstOrDefault();
            if (apartmentAsset == null)
                return null;

            return apartmentAsset;
        }

    }
}