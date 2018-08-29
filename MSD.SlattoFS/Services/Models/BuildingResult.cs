using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSD.SlattoFS.Services.Models
{
    public class BuildingResult
    {
        public BuildingResult()
        {
            AccountId = -1;
            BuildingId = -1;
        }
        public int AccountId { get; set; }
        public int BuildingId { get; set; }
    }
}