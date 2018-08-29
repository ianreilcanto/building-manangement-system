using System;
using MSD.SlattoFS.Models.Pocos.Base;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MSD.SlattoFS.Models.Pocos
{
    [TableName("SlattoSFApartmentAssets")]
    public class ApartmentAsset : IAsset
    {
        [PrimaryKeyColumn(IdentitySeed = 1, AutoIncrement = true)]
        public int Id { get; set; }

        public int ApartmentId { get; set; }

        public int MediaId { get; set; }

        [NullSetting(NullSetting = NullSettings.Null)]
        public string Url { get; set; }

        public int TypeId { get; set; }
        
        public ApartmentAsset()
        {
            TypeId = -1;
            ApartmentId = -1;
            MediaId = -1;
        }

        public ApartmentAsset(int id, int mediaId, int typeId)
        {
            TypeId = typeId;
            ApartmentId = id;
            MediaId = mediaId;
        }
    }
}
