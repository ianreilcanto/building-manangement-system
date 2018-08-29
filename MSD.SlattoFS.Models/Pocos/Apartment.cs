using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MSD.SlattoFS.Models.Pocos
{
    [TableName("SlattoSFApartments")]
    [DataContract(Name = "apartment")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class Apartment
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name of Apartment is Required")]
        [DataMember(Name = "buildingId")]
        public int BuildingId { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "status")]
        public int StatusId { get; set; }

        [DataMember(Name = "size")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Size { get; set; }

        [DataMember(Name = "numberOfRooms")]
        public int NumberOfRooms { get; set; }

        [DataMember(Name = "price")]
        public Decimal Price { get; set; }

        [DataMember(Name = "createdOn")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime? CreatedOn { get; set; }

        [DataMember(Name = "createdBy")]
        public int CreatedBy { get; set; }

        [DataMember(Name = "modifiedOn")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime? ModifiedOn { get; set; }

        [DataMember(Name = "modifiedBy")]
        public int ModifiedBy { get; set; }

        public Apartment()
        {
            BuildingId = -1;
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
            CreatedBy = -1;
            ModifiedBy = -1;
            NumberOfRooms = -1;
            Price = 0.0M;            
        }
    }
}