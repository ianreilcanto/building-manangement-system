using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSD.SlattoFS.Services.Models
{
    public class MediaFoldersResult
    {
        public MediaFoldersResult()
        {
            MediaAccountFolderId = -1;
            MediaBuildingFolderId = -1;
            MediaApartmentFolderIds = new List<int>();
        }

        public int MediaAccountFolderId { get; set; }
        public int MediaBuildingFolderId { get; set; }
        public List<int> MediaApartmentFolderIds { get; set; }
    }
}