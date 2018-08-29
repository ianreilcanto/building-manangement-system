using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using MSD.SlattoFS.Models.Pocos;

namespace MSD.SlattoFS.Models.listview
{
    [DataContract(Name = "pagedBuildings", Namespace = "")]
    public class AccountsPagedResult
    {
        [DataMember(Name = "accounts")]
        public List<Account> Accounts { get; set; }

        [DataMember(Name = "currentPage")]
        public long CurrentPage { get; set; }

        [DataMember(Name = "itemsPerPage")]
        public long ItemsPerPage { get; set; }

        [DataMember(Name = "totalPages")]
        public long TotalPages { get; set; }

        [DataMember(Name = "totalItems")]
        public long TotalItems { get; set; }
    }
}