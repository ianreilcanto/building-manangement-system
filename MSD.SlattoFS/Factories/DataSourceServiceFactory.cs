using MSD.SlattoFS.Interface;
using MSD.SlattoFS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSD.SlattoFS.Factories
{
    public static class DataSourceServiceFactory
    {
        private static IList<IDataSourceService> _dataSourceService = new List<IDataSourceService>
        {
            new ExcelDataSourceService(),
            new CSVDataSourceService()
        };

        public static IDataSourceService GetService(string type)
        {
            return _dataSourceService.Where(x => x.IsFileSupported(type)).FirstOrDefault();
        }
    }
}