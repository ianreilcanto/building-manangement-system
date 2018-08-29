using MSD.SlattoFS.Models.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSD.SlattoFS.Services.Models
{
    public class ApartmentAssignment
    {
        public Apartment Apartment { get; set; }
        public List<int> BuildingAssetIds { get; set; }
    }
     public class ApartmentAssignmentData
     {
         public List<ApartmentAssignment> AssignedApartments { get; set; }
         public List<Apartment> UnassignedApartments { get; set; }
         public int TotalApartments { get; set; }
     }
}