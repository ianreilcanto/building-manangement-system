using MSD.SlattoFS.Models.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace MSD.SlattoFS.Models.ViewModels
{
    public class SliderViewModel
    {
        public SliderViewModel()
        {
            Addresses = new List<Address>();
        }
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Address> Addresses { get; set; }
        public string Logo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }

        public string AccountName { get; set; }
        public List<Apartment> Apartments { get; set; }
        public List<ApartmentStatus> ApartmentStatuses { get; set; }

        public Guid Guid { get; set; }
        public Dictionary<int, string> MediaItems { get; set; }

    }
}