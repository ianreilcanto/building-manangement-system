using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using System.Collections.Generic;
using System.Linq;

namespace MSD.SlattoFS.Repositories
{
    public class ApartmentStatusRepository : PocoRepositoryBase<ApartmentStatus>, IPocoRepository<ApartmentStatus>
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
                return "SlattoSFApartmentStatuses";
            }
        }

        public IList<ApartmentStatus> GetAll()
        {
            return Entities;
        }

        public IList<ApartmentStatus> GetAllById(int id)
        {
            var aptAss = Entities.Where(a => a.Id == id).ToList();
            if (aptAss == null || aptAss.Count == 0)
                return new List<ApartmentStatus>();

            return aptAss;
        }
               
        public ApartmentStatus Insert(ApartmentStatus entity)
        {
            var newAsset = Database.Insert(TableName, PrimaryColumn, entity);
            if (newAsset == null)
                return null;

            return entity;
        }

        public bool Update(object id, ApartmentStatus entity)
        {
            var updateEntityCount = Database.Update(entity, id);
            return updateEntityCount > 0;
        }

        public ApartmentStatus GetById(object id)
        {
            var asset = Get(id);
            if (asset == null)
                return null;
            return asset;
        }
    }
}
