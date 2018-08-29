using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSD.SlattoFS.Services
{
    public class AddressManager
    {
        private readonly AddressRepository _addressRepo;
        public AddressManager()
        {
            _addressRepo = new AddressRepository();
        }

        public int SaveAddress(Address address)
        {
            if (!addressIsNullOrEmpty(address))
            {
                return _addressRepo.Insert(CheckForNull(address)).Id;
            }
            else
            {
                return -1;
            }
        }

        public object UpdateAddress(int id, Address address)
        {
            if (!addressIsNullOrEmpty(address))
            {
                return _addressRepo.Update(id, CheckForNull(address));
            }
            else
            {
                return null;
            }
        }

        private bool addressIsNullOrEmpty(Address address)
        {
            if ((address.Address1 == null || address.Address1 == string.Empty) &&
                (address.Address2 == null || address.Address2 == string.Empty) &&
                (address.City == null || address.City == string.Empty) &&
                (address.State_Province == null || address.State_Province == string.Empty) &&
                (address.Country == null || address.Country == string.Empty) &&
                (address.ZipCode == null || address.ZipCode == string.Empty))
                {
                return true;
                }
            else
            {
                return false;
            }
        }

        private Address CheckForNull(Address address)
        {
            if (address.Address1 == null) { address.Address1 = string.Empty; }
            if (address.Address2 == null) { address.Address2 = string.Empty; }
            if (address.City == null) { address.City = string.Empty; }
            if (address.State_Province == null) { address.State_Province = string.Empty; }
            if (address.Country == null) { address.Country = string.Empty; }
            if (address.ZipCode == null) { address.ZipCode = string.Empty; }
            return address;
        }
        public Address GetAddress(int id)
        {
            return _addressRepo.GetById(id);
        }
        public bool DeleteBuildingAdress(int addressId, int buildingId)
        {
            var count = _addressRepo.CountAddresses(buildingId);
            if (count > 1)
            {
                return _addressRepo.DeleteById(addressId);
            }
            else
            {
                return false;
            }
        }

        public List<Address> GetByBuildingId(int buildingId)
        {
            return _addressRepo.GetAllByBuildingId(buildingId);
        }

        public object DeleteByBuildingId(int buildingId)
        {
            return _addressRepo.DeleteByBuildingId(buildingId);
        }
    }
}