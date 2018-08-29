using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MSD.SlattoFS.Repositories.Result
{
    [DataContract(Name = "pagedResult")]
    public class PageResult<T>
    {
        [DataMember(Name = "items")]
        public List<T> Items { get; set; }
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
