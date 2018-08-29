using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using System.Collections.Generic;
using System.Linq;

namespace MSD.SlattoFS.Repositories
{
    public class BuildingAssetRepository : PocoRepositoryBase<BuildingAsset>, IPocoRepository<BuildingAsset>
    {
        protected override string PrimaryColumn
        {
            get
            {
                return "Id";
            }
        }

        protected override string TableName
        {
            get
            {
                return "SlattoSFBuildingAssets";
            }
        }

        public IList<BuildingAsset> GetAll()
        {
            return Entities;
        }

        public IList<BuildingAsset> GetAllById(int id)
        {
            var buildingAss = Entities.Where(a => a.BuildingId == id).ToList();
            if (buildingAss == null || buildingAss.Count == 0)
                return new List<BuildingAsset>();

            return buildingAss;
        }

        public BuildingAsset Insert(BuildingAsset entity)
        {
            var newAsset = Database.Insert(TableName, PrimaryColumn, entity);
            if (newAsset == null)
                return null;

            return entity;
        }

        public bool Update(object id, BuildingAsset entity)
        {
            var updateEntityCount = Database.Update(entity, id);
            return updateEntityCount > 0;
        }

        public BuildingAsset GetById(object id)
        {
            var asset = Entities.FirstOrDefault(a => a.MediaId.Equals(id));
            if (asset == null)
                return null;
            return asset;
        }

        public int Delete(BuildingAsset entity)
        {
           return Database.Delete(entity);
        }

        public bool DeleteByMediaId(int mediaId)
        {
            var buildingAsset = Entities.Where(a => a.MediaId == mediaId).FirstOrDefault();
            if (Delete(buildingAsset) > 0)
            {
                return true;
            }

                return false;
        }
    }
}
