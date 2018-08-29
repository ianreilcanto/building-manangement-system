using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MSD.SlattoFS.Interface
{
    public interface IDataSourceService
    {
        bool IsFileSupported(string type);
        object MapSourceToData(HttpPostedFileBase source, object id);
    }
}
