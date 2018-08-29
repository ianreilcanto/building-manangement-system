using MSD.SlattoFS.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSD.SlattoFS.Models.Pocos;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace MSD.SlattoFS.Repositories
{
    public class AddressRepository : PocoRepositoryBase<Address>, IPocoRepository<Address>
    {
        private Database _database;

        public AddressRepository()
        {
            _database = ApplicationContext.Current.DatabaseContext.Database;
        }

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
                return "SlattoSFAddresses";
            }
        }

        public IList<Address> GetAll()
        {
            return Entities;
        }

        public IList<Address> GetAllById(int id)
        {
            var addresses = GetAll().Where(a => a.Id == id).ToList();

            if (addresses == null || addresses.Count == 0)
            {
                return new List<Address>();
            }

            return addresses;
        }

        public Address Insert(Address entity)
        {
            var newApp = Database.Insert(TableName, PrimaryColumn, entity);

            if (newApp == null)
                return null;

            return entity;
        }

        public bool Update(object id, Address entity)
        {
            var updateEntityCount = Database.Update(entity, id);
            return updateEntityCount > 0;
        }

        public Address GetById(object id)
        {
            var address = Get(id);
            if (address == null)
                return null;
            return address;
        }
        public bool DeleteById(int id)
        {
            return Delete(id);
        }

        public List<Address> GetAllByBuildingId(int buildingId)
        {
            var addresses = GetAll().Where(a => a.BuildingId == buildingId).ToList();

            if (addresses == null || addresses.Count == 0)
            {
                return new List<Address>();
            }

            return addresses;
        }

        public object DeleteByBuildingId(int buildingId)
        {
            var query = string.Format("DELETE from {0} WHERE BuildingId = {1}",
                TableName,
                buildingId.ToString());
            return _database.Execute(query);
        }

        public int CountAddresses(int buildingId)
        {
            var count = _database.Fetch<int>("SELECT COUNT(*) FROM SlattoSFAddresses where BuildingId = " + buildingId).FirstOrDefault();
            return count;
        }
    }
}
