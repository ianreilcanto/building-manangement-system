using MSD.SlattoFS.Models.Pocos;
using System.Collections.Generic;

namespace MSD.SlattoFS.Models.ViewModels
{
    public class ApartmentAssetViewModel
    {
        public string Url { get; set; }
        public int MediaId { get; set; }
        public ApartmentAssetViewModel()
        {
                  
        }
    }

    public class BaseApartmentViewModel
    {
        public int Id { get; set; }
        public string Room { get; set; }
    }

    public class ApartmentViewModel : BaseApartmentViewModel
    {
        public int BuildingId { get; set; }
        public int StatusId { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public List<ApartmentAssetViewModel> FileAssets { get; set; }

        public ApartmentViewModel()
        {
            Id = -1;
            Room = "Unknown";
            BuildingId = -1;
            StatusId = -1;
            Price = 0;
            Size = "Unknown";
            FileAssets = new List<ApartmentAssetViewModel>();
        }

        public static ApartmentViewModel CreateModel(Apartment apartment)
        {
            var model = new ApartmentViewModel();
            model.Id = apartment.Id;
            model.BuildingId = apartment.BuildingId;
            model.Room = apartment.Name;
            model.StatusId = apartment.StatusId;
            model.Price = apartment.Price;
            model.Size = apartment.Size;
            return model;
        }

    }
}