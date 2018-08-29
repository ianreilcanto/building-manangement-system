using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MSD.SlattoFS.Models.Pocos
{
    [TableName("SlattoSFApartmentStatuses")]
    public class ApartmentStatus
    {
        [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
