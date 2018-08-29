using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Models.Pocos.Base;
using System;

namespace MSD.SlattoFS.Factories
{
    public enum AssetType
    {
        Unknown = 0,
        Building,
        Apartment
    }

    public enum AssetMediaType
    {
        Unknown = 0,
        Image,
        File
    }

    public static class AssetFactory
    {
        public static IAsset CreateAsset(AssetType type, int id, int mediaId, AssetMediaType mediaType)
        {
            var typeId = (int)type;
            var mediaTypeId = (int)mediaType;
            switch (type)
            {
                case AssetType.Building:
                    return new BuildingAsset(id, mediaId, mediaTypeId);
                case AssetType.Apartment:
                    return new ApartmentAsset(id, mediaId, mediaTypeId);
                default:
                    return new BuildingAsset(id, mediaId, mediaTypeId);
            }
            
        }
    }
}