using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MSD.SlattoFS.Models.Pocos
{
    [TableName("SlattoSFApartmentFolders")]
    public class ApartmentFolder
    {
        [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
        public int Id { get; set; }

        public int ApartmentId { get; set; }
        public int FolderId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public ApartmentFolder()
        {
            ApartmentId = -1;
            FolderId = -1;
            Name = "";
            Url = "";
        }
    }
}
