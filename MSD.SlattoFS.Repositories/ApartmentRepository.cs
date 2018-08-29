using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace MSD.SlattoFS.Repositories
{
    public class ApartmentRepository : PocoRepositoryBase<Apartment>, IPocoRepository<Apartment>
    {
        private Database _database;
        public ApartmentRepository()
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
                return "SlattoSFApartments";
            }
        }

        public IList<Apartment> GetAll()
        {
            return Entities;
        }

        public IList<Apartment> GetAllById(int id)
        {
            var apartment = Entities.Where(a => a.Equals(id)).ToList();
            if (apartment == null || apartment.Count == 0)
                return new List<Apartment>();

            return apartment;
        }

        public Apartment GetByName(string name)
        {
            var apartment = GetAll()
                .Where(a => a.Name == name).FirstOrDefault();

            return apartment;

        }

        public Apartment GetByNameAndBuildingId(string name , int id)
        {
            var apartment = GetAll()
               .Where(a => a.Name == name && a.BuildingId == id).FirstOrDefault();

            return apartment;
        }

        public Apartment Insert(Apartment entity)
        {
            var newApp = Database.Insert(TableName, PrimaryColumn, entity);

            if (newApp == null)
                return null;

            return entity;
        }

        public bool Update(object id, Apartment entity)
        {
            var updateEntityCount = Database.Update(entity, id);
            return updateEntityCount > 0;
        }

        public Apartment GetById(object id)
        {
            var apartment = Get(id);
            if (apartment == null)
                return null;
            return apartment;
        }

        public List<Apartment> GetAllByBuildingId(int buildingId)
        {
            var apartments = GetAll().Where(a => a.BuildingId == buildingId).ToList();

            if (apartments == null || apartments.Count == 0)
            {
                return new List<Apartment>();
            }

            return apartments;
        }
        public int CountApartments(int buildingId)
        {
            var query = string.Format("SELECT COUNT(*) FROM {0} WHERE BuildingId = {1}",
                TableName,
                buildingId.ToString());
            var count = _database.Fetch<int>(query).FirstOrDefault();
            return count;
        }
        public List<Apartment> GetByIds(List<int> aptIds)
        {
            if (aptIds.Count == 0)
                return new List<Apartment>();

            var query = string.Format("SELECT * FROM {0} WHERE Id IN(", TableName);

            for(int i = 0;i<aptIds.Count;i++)
            {
                query += aptIds[i].ToString();
                query += (i == aptIds.Count - 1) ? ")" : ",";
            }

            var apartments = _database.Fetch<Apartment>(query);
            return apartments;
        }

        public List<Apartment> GetAllExcept(List<int> aptIds)
        {
            if (aptIds.Count == 0)
                return new List<Apartment>();

            var query = string.Format("SELECT * FROM {0} WHERE Id NOT IN(", TableName);

            for (int i = 0; i < aptIds.Count; i++)
            {
                query += aptIds[i].ToString();
                query += (i == aptIds.Count - 1) ? ")" : ",";
            }

            var apartments = _database.Fetch<Apartment>(query);
            return apartments;
        }
    }
}
