using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MSD.SlattoFS.Models.Pocos
{
    [TableName("SlattoSFSVGData")]
    public class SvgData
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        public string Svg { get; set; }

        public int BuildingId { get; set; }

        public int AssetId { get; set; }

        public int Type { get; set; }
    }
}