using System.Runtime.Serialization;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MSD.SlattoFS.Models.Pocos
{
    [TableName("SlattoSFAddresses")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class Address
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "buildingId")]
        public int BuildingId { get; set; }

        [DataMember(Name = "address1")] // (Street address, P.O. box, company name, c/o )
        public string Address1 { get; set; }

        [DataMember(Name = "address2")] //(Apartment, suite, unit, building, floor, etc.)
        public string Address2 { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "state_province")]
        public string State_Province { get; set; }

        [DataMember(Name = "country")]
        public string Country { get; set; }

        [DataMember(Name = "zipCode")]
        public string ZipCode { get; set; }

    }
}
