using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSD.SlattoFS.Interface
{
    public interface ISVGDataSource
    {
        string GetSVGData(int buildingId, int assetId);
    }
}
