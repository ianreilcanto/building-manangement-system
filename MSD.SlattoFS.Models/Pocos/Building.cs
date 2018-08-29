using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MSD.SlattoFS.Models.Pocos
{
    [TableName("SlattoSFBuildings")]
    [PrimaryKey("Id", autoIncrement = true)]
    [DataContract(Name = "building")]
    public class Building
    {
        public Building() {
            this.Guid = Guid.NewGuid();
        }

        [PrimaryKeyColumn(AutoIncrement = true)]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "accountId")]
        public int AccountId { get; set; }

        [DataMember(Name = "name")]
        [Required(ErrorMessage = "Building name is required.")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        [Required(ErrorMessage = "Building Description is required.")]
        public string Description { get; set; }

        [DataMember(Name = "createdOn")]
        public DateTime? CreatedOn { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int CreatedBy { get; set; }

        [DataMember(Name = "modifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [DataMember(Name = "createdBy")]
        public int ModifiedBy { get; set; }

        [DataMember(Name = "guid")]
        public Guid Guid { get; set; }
    }
}