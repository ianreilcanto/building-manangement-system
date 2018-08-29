using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSD.SlattoFS.Repositories
{
    public class BuildingRepository : PocoRepositoryBase<Building>, IPocoRepository<Building>
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
                return "SlattoSFBuildings";
            }
        }

        public IList<Building> GetAll()
        {
            return Entities;
        }
        
        public IList<Building> GetAllById(int id)
        {
            var buildings = Entities.Where(a => a.Id.Equals(id)).ToList();
            if (buildings == null || buildings.Count == 0)
                return new List<Building>();

            return buildings;
        }

        public Building GetByName(string name)
        {
            var building = GetAll()
               .Where(a => a.Name == name).FirstOrDefault();
            return building;
        }

        public Building Insert(Building entity)
        {
            var newBuilding = Database.Insert(TableName, PrimaryColumn, entity);
            if (newBuilding == null)
                return null;

            return entity;
        }

        public bool Update(object id, Building entity)
        {
            var updateEntityCount = Database.Update(entity, id);
            return updateEntityCount > 0;
        }

        public Building GetById(object id)
        {
            var building = Get(id);
            if (building == null)
                return null;
            return building;
        }

        public bool DeleteById(int id)
        {
            return Delete(id);
        }

        public Building GetByGuid(Guid guid)
        {
            var building = GetAll()
                .FirstOrDefault(a => a.Guid == guid);
            return building;
        }
    }
}
