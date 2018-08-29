using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MSD.SlattoFS.Models.Pocos
{
    [TableName("SlattoSFAccountFolders")]
    public class AccountFolder
    {
        [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
        public int Id { get; set; }

        public int AccountId { get; set; }
        public int FolderId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public AccountFolder()
        {
            AccountId = -1;
            FolderId = -1;
            Name = "";
            Url = "";
        }
    }
}
