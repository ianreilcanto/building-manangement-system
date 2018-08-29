using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.DynamicData;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MSD.SlattoFS.Models.Pocos
{

    [TableName("SlattoSFBuildingAssets")]
    [DataContract(Name = "buildingAssets")]
    public class BuildingAssets
    {
        /*
            NOTE: please read. 
             This is a deprecated POCO and is replaced by 'BuildingAsset' object.
             THis is due to the redefinition of the BuildingAsset object to reference just the MediaId which is 
             a property to get the MediaLibrary Id of the node/content in umbraco
             */
        [PrimaryKeyColumn(AutoIncrement = true)]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "imagePath")]
        public string ImagePath { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "type")]     
        public string Type { get; set; }

        [DataMember(Name = "buildingId")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public int? BuildingId { get; set; }

        [DataMember(Name = "apartmentId")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public int? ApartmentId { get; set; }

    }
}
