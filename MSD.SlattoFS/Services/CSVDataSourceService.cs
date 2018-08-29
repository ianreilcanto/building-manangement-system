using MSD.SlattoFS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSD.SlattoFS.Services
{
    public class CSVDataSourceService : IDataSourceService
    {
        private List<string> FILE_EXTENSTIONS = new List<string> { ".txt" };
        public void ReadAndConvertData()
        {
            throw new NotImplementedException();
        }
        public bool IsFileSupported(string type)
        {
            return FILE_EXTENSTIONS.Any(x => x.Equals(type, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsValidDataSource(HttpPostedFileBase source)
        {
            //TODO: not handled yet
            return true;
        }

        public object MapSourceToData(HttpPostedFileBase source, object id)
        {

            if (IsValidDataSource(source))
            {
                throw new InvalidOperationException("Uploaded file source is invalid.");
            }

            return null;
        }
    }
}