using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSD.SlattoFS.Interface
{
    public interface IUploadManager
    {
        void Upload(Stream stream, string filename, string destinationPath);
    }
}
