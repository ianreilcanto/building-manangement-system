using MSD.SlattoFS.Interface;
using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSD.SlattoFS.Services
{
    public class SVGDataSource : ISVGDataSource
    {
        private readonly SVGDataRepository _svgRepo;

        public SVGDataSource()
        {
            _svgRepo = new SVGDataRepository();
        }

        public string GetSVGData(int buildingId, int assetId)
        {
            var svgData = _svgRepo.GetAllById(buildingId).Where(a => a.AssetId == assetId).FirstOrDefault();

            if (svgData == null)
            {
                svgData = new SvgData();
                svgData.Svg = "<script>alert(\"No SVG Found\")<\\script>";
            }
            return svgData.Svg;
        }

        public bool DeleteByAssetId(int assetId)
        {
            return ((SVGDataRepository)_svgRepo).DeleteByAssetId(assetId);
        }
    }
}