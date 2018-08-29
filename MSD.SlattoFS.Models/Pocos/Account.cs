using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using Umbraco.Core.Persistence;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace MSD.SlattoFS.Models.Pocos
{
    [TableName("SlattoSFAccounts")]
    [DataContract(Name = "account")]
    public class Account
    {
        public Account() { }

        [PrimaryKeyColumn(AutoIncrement = true)]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name of Account is Required")]
        [DataMember(Name = "name")]
        public string Name { get; set; }
        //public int UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email of Account is Required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "imageId")]
        public int ImageId { get; set; }

        [DataMember(Name = "createdOn")]
        public DateTime CreatedOn { get; set; }

        [DataMember(Name = "lastModifiedOn")]
        public DateTime LastModifiedOn { get; set; }

        [DataMember(Name = "createdBy")]
        public int CreatedBy { get; set; }

        [DataMember(Name = "modifiedBy")]
        public int ModifiedBy { get; set; }

        [DataMember(Name = "userId")]
        public int UserId { get; set; }

        [DataMember(Name = "memberId")]
        public int MemberId { get; set; }
    }
}