using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSD.SlattoFS.Repositories
{
    public class ApartmentFolderRepository : PocoRepositoryBase<ApartmentFolder>, IPocoRepository<ApartmentFolder>
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
                return "SlattoSFApartmentFolders";
            }
        }

        public IList<ApartmentFolder> GetAll()
        {
            return Entities;
        }

        public IList<ApartmentFolder> GetAllById(int id)
        {
            var apartmentFolders = Entities.Where(a => a.Id.Equals(id)).ToList();
            if (apartmentFolders == null || apartmentFolders.Count == 0)
                return new List<ApartmentFolder>();

            return apartmentFolders;
        }

        public ApartmentFolder GetById(object id)
        {
            var apartmentFolder = Get(id);
            if (apartmentFolder == null)
                return null;
            return apartmentFolder;
        }

        public ApartmentFolder Insert(ApartmentFolder entity)
        {
            var newAptFolder = Database.Insert(TableName, PrimaryColumn, entity);
            if (newAptFolder == null)
                return null;

            return entity;
        }

        public bool Update(object id, ApartmentFolder entity)
        {
            var updateEntityCount = Database.Update(entity, id);
            return updateEntityCount > 0;
        }
    }
}
