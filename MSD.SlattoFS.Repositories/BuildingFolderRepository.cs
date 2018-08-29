using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSD.SlattoFS.Repositories
{
    public class BuildingFolderRepository : PocoRepositoryBase<BuildingFolder>, IPocoRepository<BuildingFolder>
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
                return "SlattoSFBuildingFolders";
            }
        }

        public IList<BuildingFolder> GetAll()
        {
            return Entities;
        }

        public IList<BuildingFolder> GetAllById(int id)
        {
            var buildingFolders = Entities.Where(a => a.Id.Equals(id)).ToList();
            if (buildingFolders == null || buildingFolders.Count == 0)
                return new List<BuildingFolder>();

            return buildingFolders;
        }

        public BuildingFolder GetById(object id)
        {
            var buildingFolder = Get(id);
            if (buildingFolder == null)
                return null;
            return buildingFolder;
        }

        public BuildingFolder Insert(BuildingFolder entity)
        {
            var newBuildingFolder = Database.Insert(TableName, PrimaryColumn, entity);
            if (newBuildingFolder == null)
                return null;

            return entity;
        }

        public bool Update(object id, BuildingFolder entity)
        {
            var updateEntityCount = Database.Update(entity, id);
            return updateEntityCount > 0;
        }
    }
}
