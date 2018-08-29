using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace MSD.SlattoFS.Models.ViewModels
{
    public class BuildingConfigViewModel : RenderModel
    {
        public BuildingConfigViewModel(IPublishedContent content): base(content)
        {
            AccountId = -1;
            BuildingId = -1;
        }

        public int AccountId { get; set; }
        public int BuildingId { get; set; }
    }

    [DataContract]
    public class BuildingSvgConfigModel
    {
        [DataMember(Name = "buildingId")]
        public int BuildingId { get; set; }
        [DataMember(Name = "assetId")]
        public int AssetId { get; set; }
        [DataMember(Name = "svg")]
        public string Svg { get; set; }
    }
}