using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using System.Collections.Generic;
using System.Linq;

namespace MSD.SlattoFS.Repositories
{
    public class ApartmentAssetRepository : PocoRepositoryBase<ApartmentAsset>, IPocoRepository<ApartmentAsset>
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
                return "SlattoSFApartmentAssets";
            }
        }

        public IList<ApartmentAsset> GetAll()
        {
            return Entities;
        }

        public IList<ApartmentAsset> GetAllById(int id)
        {
            var aptAss = Entities.Where(a => a.ApartmentId == id).ToList();
            if (aptAss == null || aptAss.Count == 0)
                return new List<ApartmentAsset>();

            return aptAss;
        }
               
        public ApartmentAsset Insert(ApartmentAsset entity)
        {
            var newAsset = Database.Insert(TableName, PrimaryColumn, entity);
            if (newAsset == null)
                return null;

            return entity;
        }

        public bool Update(object id, ApartmentAsset entity)
        {
            var updateEntityCount = Database.Update(entity, id);
            return updateEntityCount > 0;
        }

        public ApartmentAsset GetById(object id)
        {
            var asset = Get(id);
            if (asset == null)
                return null;
            return asset;
        }
    }
}
