using MSD.SlattoFS.Interface;
using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Repositories;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Umbraco.Core.Logging;

namespace MSD.SlattoFS.Services
{
    public class ExcelDataSourceService : IDataSourceService
    {
        private readonly IPocoRepository<Apartment> _apartmentRepo;
        private readonly IPocoRepository<ApartmentAsset> _apartmentAssetRepo;
        private readonly IPocoRepository<ApartmentStatus> _apartmentStatusRepo;

        private List<string> FILE_EXTENSTIONS = new List<string> { ".xlsx", ".xls" };

        public ExcelDataSourceService()
        {
            _apartmentRepo = new ApartmentRepository();
            _apartmentAssetRepo = new ApartmentAssetRepository();
            _apartmentStatusRepo = new ApartmentStatusRepository();

        }

        public bool IsFileSupported(string type)
        {
            return FILE_EXTENSTIONS.Any(x => x.Equals(type, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsValidDataSource(HttpPostedFileBase source)
        {
            if (source == null || source.ContentLength == 0) return false;
            try
            {
                ExcelPackage package = new ExcelPackage(source.InputStream);
                ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            }
            catch (Exception ex)
            {
                LogHelper.Error<ExcelDataSourceService>(ex.Message, ex);
                return false;
            }

            return true;
        }

        public object MapSourceToData(HttpPostedFileBase source, object id)
        {
            try
            {
                if (!IsValidDataSource(source))
                {
                    throw new InvalidOperationException("Uploaded file source is invalid.");
                }

                ExcelPackage package = new ExcelPackage(source.InputStream);
                ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

                List<Apartment> apartments = new List<Apartment>();
                DataTable table = ConvertToDataTable(workSheet);
                apartments = MapData(table, (int)id);
                return apartments;
            }
            catch (Exception ex)
            {
                LogHelper.Error<ExcelDataSourceService>(ex.Message, ex);
                return null;
            }
        }

        private ApartmentStatus GetStatus(string type)
        {
            var status = _apartmentStatusRepo.GetAll().FirstOrDefault(x => x.Name.Contains(type));
            return status;
        }

        private DataTable ConvertToDataTable(ExcelWorksheet workSheet)
        {
            try
            {
                DataTable table = new DataTable();
                //create the table columns
                foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                {
                    table.Columns.Add(new DataColumn { ColumnName = firstRowCell.Text });
                }

                //convert each row to datarow
                for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                {
                    var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                    var newRow = table.NewRow();
                    foreach (var cell in row)
                    {
                        newRow[cell.Start.Column - 1] = cell.Text;
                    }
                    table.Rows.Add(newRow);
                }

                return table;
            }
            catch (Exception ex)
            {
                LogHelper.Error<ExcelDataSourceService>(ex.Message, ex);
                return null;
            }
        }

        private List<Apartment> MapData(DataTable table, int id= -1)
        {
            var list = new List<Apartment>(table.Rows.Count - 1);
            foreach(DataRow row in table.AsEnumerable())
            {
                var values = row.ItemArray;
                var existingApt = _apartmentRepo
                       .GetAll()
                       .FirstOrDefault(a => a.Name.Equals(values[0].ToString(), StringComparison.OrdinalIgnoreCase) && a.BuildingId == id);

                if (existingApt != null)
                {
                    existingApt.Name = values[0].ToString();
                    existingApt.StatusId = GetStatus(values[1].ToString()) != null ? GetStatus(values[1].ToString()).Id : -1;
                    existingApt.Size = values[2].ToString();
                    existingApt.NumberOfRooms = int.Parse(values[3].ToString());
                    existingApt.Price = decimal.Parse(values[4].ToString());
                    existingApt.BuildingId = id;
                    var isUpdatedApt = _apartmentRepo.Update(existingApt.Id, existingApt);
                    if(isUpdatedApt)
                    {
                        list.Add(existingApt);
                    }
                    continue;
                }
                else
                {
                    var apartment = new Apartment();
                    apartment.Name = values[0].ToString();
                    apartment.StatusId = GetStatus(values[1].ToString()) != null ? GetStatus(values[1].ToString()).Id : -1;
                    apartment.Size = values[2].ToString();
                    apartment.NumberOfRooms = int.Parse(values[3].ToString());
                    apartment.Price = decimal.Parse(values[4].ToString());
                    apartment.BuildingId = id;
                    var newApartment = _apartmentRepo.Insert(apartment);
                    if(newApartment != null)
                    {
                        list.Add(newApartment);
                    }
                    continue;
                }
            }
            return list;
        }

    }
}