using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Models.Pocos.Base;
using System.Collections.Generic;

namespace MSD.SlattoFS.Models.ViewModels
{
    public class BuildingInformation
    {
        public BuildingInformation()
        {
            Assets = new List<IAsset>();
            Addresses = new List<Address>();
        }

        public BuildingInformation(int id, string name, string description) : this()
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public int Id { get; set; }   
        public string Name { get; set; }
        public string Description { get; set; }  
        public IList<Address> Addresses { get; set; }        
        public IList<IAsset> Assets { get; set; }

        public void SetDefaultAsset(IAsset asset)
        {
            if (asset == null)
                asset = new BuildingAsset();

            if(Assets == null)
            {
                Assets = new List<IAsset>();
            }

            Assets.Add(asset);
        }

        public static BuildingInformation CreateModel(int id, string name, string description = "")
        {
            var model = new BuildingInformation(id, name, description);
            return model;
        }

    }
}