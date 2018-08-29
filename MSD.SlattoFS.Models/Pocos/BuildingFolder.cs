using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MSD.SlattoFS.Models.Pocos
{
    [TableName("SlattoSFBuildingFolders")]
    public class BuildingFolder
    {
        [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
        public int Id { get; set; }

        public int AccountId { get; set; }
        public int BuildingId { get; set; }
        public int FolderId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public BuildingFolder()
        {
            AccountId = -1;
            BuildingId = -1;
            FolderId = -1;
            Name = "";
            Url = "";
        }
    }
}
