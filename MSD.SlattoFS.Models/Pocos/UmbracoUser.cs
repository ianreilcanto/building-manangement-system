using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace MSD.SlattoFS.Models.Pocos
{
    public class UmbracoUser
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}