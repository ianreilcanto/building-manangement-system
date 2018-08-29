using MSD.SlattoFS.App_Plugins.Buildings.Controllers.Models;
using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace MSD.SlattoFS.App_Plugins.Buildings.Controllers
{
    [PluginController("Buildings")]
    public class AddressApiController : UmbracoAuthorizedJsonController
    {
        private readonly IPocoRepository<Address> _addressRepo;

        public AddressApiController()
        {
            _addressRepo = new AddressRepository();
        }
        public Address GetById(int id)
        {

            var query = new Sql().Select("*").From("SlattoSFAddresses").Where<Address>(x => x.Id == id);
            return DatabaseContext.Database.Fetch<Address>(query).FirstOrDefault();

        }

        public Address PostSave(Address address)
        {
            if (address.Id > 0)
            {
                //DatabaseContext.Database.Update(address);.
                _addressRepo.Update(address.Id, address);
                return address;
            }
            else
            {
                return _addressRepo.Insert(address);
                //  DatabaseContext.Database.Save(address);
            }
        }

        public int DeleteById(int id)
        {
            var count = DatabaseContext.Database.Fetch<int>("SELECT COUNT(*) FROM SlattoSFAddresses where BuildingId = (SELECT BuildingId FROM SlattoSFAddresses WHERE Id = " + id + "); ").FirstOrDefault();
            if (count > 1)
                return DatabaseContext.Database.Delete<Address>(id);
            else
                return 0;

        }

        public BuildingBackOfficeModel AddBlankAddress(BuildingBackOfficeModel model)
        {
            Address blankAddress = new Address();
            blankAddress.Id = 0;
            blankAddress.BuildingId = model.Building == null ? 0 : model.Building.Id;
            blankAddress.Address1 = string.Empty;
            blankAddress.Address2 = string.Empty;
            blankAddress.City = string.Empty;
            blankAddress.State_Province = string.Empty;
            blankAddress.Country = string.Empty;
            blankAddress.ZipCode = string.Empty;
            
            if (model.Addresses != null)
            {
                model.Addresses.Add(blankAddress);
            }
            else
            {
                List<Address> addresses = new List<Address>();
                addresses.Add(blankAddress);
                model.Addresses = addresses;
            }

            return model;
        }
    }
}