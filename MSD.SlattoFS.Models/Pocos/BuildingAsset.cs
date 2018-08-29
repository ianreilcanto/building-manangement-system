using MSD.SlattoFS.Models.Pocos.Base;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MSD.SlattoFS.Models.Pocos
{
    [TableName("SlattoSFBuildingAssets")]
    public class BuildingAsset : IAsset
    {
        [PrimaryKeyColumn(IdentitySeed = 1, AutoIncrement = true)]
        public int Id { get; set; }

        public int BuildingId { get; set; }

        public int MediaId { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string Url { get; set; }

        public int TypeId { get; set; }

        public BuildingAsset()
        {
            TypeId = -1;
            BuildingId = -1;
            MediaId = -1;
        }

        public BuildingAsset(int id, int mediaId, int typeId)
        {
            TypeId = typeId;
            BuildingId = id;
            MediaId = mediaId;
        }

    }
}
