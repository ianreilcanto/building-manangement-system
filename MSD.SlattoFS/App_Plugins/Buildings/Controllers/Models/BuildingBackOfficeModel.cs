using MSD.SlattoFS.Models.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MSD.SlattoFS.App_Plugins.Buildings.Controllers.Models
{
    [DataContract]
    public class BuildingBackOfficeModel
    {
        [DataMember(Name="building")]
        public Building Building { get; set; }

        [DataMember(Name="addressList")]
        public List<Address> Addresses { get; set; }
    }
}