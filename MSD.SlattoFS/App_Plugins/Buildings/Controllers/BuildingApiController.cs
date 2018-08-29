using MSD.SlattoFS.App_Plugins.Buildings.Controllers.Models;
using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.listview;
using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Repositories;
using MSD.SlattoFS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Persistence;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace MSD.SlattoFS.App_Plugins.Buildings.Controllers
{
    [PluginController("Buildings")]
    public class BuildingApiController : UmbracoAuthorizedJsonController
    {
        private readonly IPocoRepository<Building> _bldgRepo;
        private readonly IPocoRepository<Address> _addressRepo;

        public BuildingApiController()
        {
            _bldgRepo = new BuildingRepository();
            _addressRepo = new AddressRepository();
        }

        public IEnumerable<Building> GetAll()
        {
            return _bldgRepo.GetAll();
        }

        public BuildingBackOfficeModel GetById(int id)
        {
            BuildingBackOfficeModel model = new BuildingBackOfficeModel();
            var building = _bldgRepo.GetById(id);
            AddressManager addressMgr = new AddressManager();

            model.Building = building;
            model.Addresses = addressMgr.GetByBuildingId(building.Id);
            return model;
        }

        public BuildingBackOfficeModel PostSave(BuildingBackOfficeModel model)
        {
            var building = model.Building;
            var addresses = model.Addresses;
            umbraco.BusinessLogic.User siteAdmin = umbraco.helper.GetCurrentUmbracoUser();

            if (building.Id > 0)
            {
                building.ModifiedBy = siteAdmin.Id;
                building.ModifiedOn = DateTime.Now;
                _bldgRepo.Update(building.Id, building);
            }
            else
            {
                building.CreatedBy = siteAdmin.Id;
                building.CreatedOn = DateTime.Now;
                building.ModifiedBy = siteAdmin.Id;//should be nullable BM-21 fix
                building.ModifiedOn = DateTime.Now;//should be nullable BM-21 fix
                var buildingInserted = _bldgRepo.Insert(building);
                building.Id = buildingInserted.Id;
            }

            foreach (var address in addresses)
            {
                if (address.Id > 0)
                {
                    //DatabaseContext.Database.Update(address);
                    _addressRepo.Update(address.Id, address);
                }
                else
                {
                    if (!IsCanceledAddress(address))
                    {
                        address.BuildingId = building.Id;
                        //DatabaseContext.Database.Save(address);
                        _addressRepo.Insert(address);
                    }
                }
            }

            return model;
        }

        public bool DeleteById(int id)
        {
            var model = GetById(id);
            var result = false;

            if (model.Building != null)
            {
                result = ((BuildingRepository)_bldgRepo).DeleteById(id);
                ((AddressRepository)_addressRepo).DeleteByBuildingId(id);
            }

            return result;
        }

        public int PostDeleteByBatch(int[] ids)
        {
            foreach (var id in ids)
            {
                DeleteById(id);
            }

            return ids.Count();
        }

        public PagedResult GetPaged(int itemsPerPage, int pageNumber, string sortColumn,
        string sortOrder, string searchTerm)
        {
            var items = new List<Building>();
            var db = DatabaseContext.Database;
            var currentType = typeof(Building);

            var query = new Sql().Select("*").From("SlattoSFBuildings");

            if (!string.IsNullOrEmpty(searchTerm))
            {
                int c = 0;
                foreach (var property in currentType.GetProperties())
                {
                    string before = "WHERE";
                    if (c > 0)
                    {
                        before = "OR";
                    }

                    var columnAttri = property.GetCustomAttributes(typeof(ColumnAttribute), false);

                    var columnName = property.Name;
                    if (columnAttri.Any())
                    {
                        columnName = ((ColumnAttribute)columnAttri.FirstOrDefault()).Name;
                    }

                    query.Append(before + " [" + columnName + "] like @0", "%" + searchTerm + "%");
                    c++;
                }
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortOrder))
                query.OrderBy(sortColumn + " " + sortOrder);
            else
                query.OrderBy("id asc");

            var p = db.Page<Building>(pageNumber, itemsPerPage, query);
            var result = new PagedResult
            {
                TotalPages = p.TotalPages,
                TotalItems = p.TotalItems,
                ItemsPerPage = p.ItemsPerPage,
                CurrentPage = p.CurrentPage,
                Buildings = p.Items.ToList()
            };
            return result;
        }

        private bool IsCanceledAddress(Address address)
        {
            bool address1IsCancelled = address.Address1 == null || address.Address1 == string.Empty;
            bool address2IsCancelled = address.Address2 == null || address.Address2 == string.Empty;
            bool cityIsCancelled = address.City == null || address.City == string.Empty;
            bool stateProvinceIsCancelled = address.State_Province == null || address.State_Province == string.Empty;
            bool countryIsCancelled = address.Country == null || address.Country == string.Empty;
            bool zipCodeIsCancelled = address.ZipCode == null || address.ZipCode == string.Empty;

            return address1IsCancelled && address2IsCancelled && cityIsCancelled && stateProvinceIsCancelled && countryIsCancelled && zipCodeIsCancelled;
        }

    }
}