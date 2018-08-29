using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace MSD.SlattoFS.Models.ViewModels
{
    public class BuildingsViewModel : RenderModel
    {
        public BuildingsViewModel(IPublishedContent content) : base(content)
        {
            Buildings = new List<BuildingInformation>();
        }
        public IEnumerable<int> BuildingIds { get; set; }
        public string Greeting { get { return "From Buildings VIew Model CUstom prop"; } }

        public List<BuildingInformation> Buildings { get; set; }
    }
}