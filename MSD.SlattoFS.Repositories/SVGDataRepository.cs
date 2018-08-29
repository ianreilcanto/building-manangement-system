using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSD.SlattoFS.Repositories
{
    public class SVGDataRepository : PocoRepositoryBase<SvgData>, IPocoRepository<SvgData>
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
                return "SlattoSFSVGData";
            }
        }

        public IList<SvgData> GetAll()
        {
            return Entities;
        }

        public IList<SvgData> GetAllById(int id)
        {
            var svg = GetAll().Where(a => a.BuildingId == id).ToList();
            if (svg == null || svg.Count == 0)
            {
                return new List<SvgData>();
            }

            return svg;
        }

        public IList<SvgData> GetByBuildingId(int id)
        {
            var svg = GetAll().Where(a => a.BuildingId == id).ToList();
            if (svg == null || svg.Count == 0)
            {
                return new List<SvgData>();
            }

            return svg;
        }

        public SvgData GetById(object id)
        {
            var asset = Get(id);
            if (asset == null)
            {
                return null;
            }
            return asset;
        }

        public SvgData Insert(SvgData entity)
        {
            var newApp = Database.Insert(TableName, PrimaryColumn, entity);
            if (newApp == null)
            {
                return null;
            }

            return entity;
        }

        public bool Update(object id, SvgData entity)
        {
            var updateEntityCount = Database.Update(entity, id);

            return updateEntityCount > 0;
        }

        public int Delete(SvgData entity)
        {
            return Database.Delete(entity);
        }

        public bool DeleteByAssetId(int assetId)
        {
            var svgData = Entities.Where(s => s.AssetId == assetId).FirstOrDefault();
            if (svgData == null)
            {
                return true;
            }
            if (Delete(svgData) > 0)
            {
                return true;
            }

            return false;
        }
    }
}