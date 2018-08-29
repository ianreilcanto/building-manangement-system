using MSD.SlattoFS.Factories;
using MSD.SlattoFS.Interface;
using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Models.Pocos.Base;
using MSD.SlattoFS.Repositories;
using MSD.SlattoFS.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace MSD.SlattoFS.Services
{
    public class MediaManager : IUploadManager
    {
        private readonly UmbracoContext _context;

        private readonly IPocoRepository<AccountFolder> _accountFolderRepo;
        private readonly IPocoRepository<BuildingFolder> _buildingFolderRepo;
        private readonly IPocoRepository<BuildingAsset> _buildingAssetRepo;
        private readonly IPocoRepository<ApartmentAsset> _apartmentAssetRepo;

        private readonly IPocoRepository<ApartmentFolder> _apartmentFolderRepo;
        private readonly IPocoRepository<Building> _buildingRepo;
        private readonly IPocoRepository<Account> _accountRepo;
        private readonly ApartmentRepository _apartmentRepo;

        private readonly AssetManager _assetManager;
        private readonly IMediaService _mediaService;
        private readonly UmbracoHelper _umbracoHelper;
        private const string FILE_KEY = "file";

        private const string ACCOUNTID_KEY = "accountId";
        private const string BUILDINGID_KEY = "buildingId";
        private string[] IMAGEFORMATS_SUPPORTED = new string[] { ".jpg", ".gif", ".jpeg", ".png" };
        private string[] FILEFORMATS_SUPPORTED = new string[] { ".pdf", ".xlsx", ".xls", ".txt", ".csv" };

        public MediaManager(UmbracoContext context)
        {
            _context = context;
            _accountFolderRepo = new AccountFolderRepository();
            _buildingFolderRepo = new BuildingFolderRepository();
            _buildingAssetRepo = new BuildingAssetRepository();

            _apartmentAssetRepo = new ApartmentAssetRepository();
            _apartmentFolderRepo = new ApartmentFolderRepository();
            _buildingRepo = new BuildingRepository();
            _accountRepo = new AccountRepository();

            _apartmentRepo = new ApartmentRepository();
            _assetManager = new AssetManager();
            _mediaService = _context.Application.Services.MediaService;
            _umbracoHelper = new UmbracoHelper(_context);
        }

        public IMedia GetMediaAsset(int id)
        {
            return _mediaService.GetById(id);
        }

        /// <summary>
        /// Method to check if asset is found on db and that if found,
        /// check if the reference to media library object can be found also,
        /// otherwise, delete the record on the database and default to 'no image' thumbnail
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public IMedia TryGetTypedMediaAsset(int buildingId, int mediaId)
        {
            //try finding from the custom 'buildingAsset' db table
            var dbMediaAsset = _buildingAssetRepo.GetById(mediaId);

            //from the umbraco media library
            var mediaAsset = _mediaService.GetById(mediaId); // UmbracoHelper.TypedMedia(mediaId);

            //if dbasset found and media (from umbraco) also found
            if (dbMediaAsset != null && (mediaAsset != null && !mediaAsset.Trashed))
            {
                return mediaAsset;
            }

            IMedia mediaAssetFound = null;

            //Building
            var building = _buildingRepo.GetById(buildingId);
            var account = _accountRepo.GetById(building.AccountId);

            //if it CANNOT find the DB asset
            if (dbMediaAsset == null)
            {
                //TODO: what if there are many folders existing for the building
                //try to check if building has already existing DB folder
                var bldgFolder = GetBuildingFolder(building.Id);
                if (bldgFolder != null)
                {
                    //try to check if media library folder for the building already exist in UMB
                    var mediaAssetFolder = GetMediaFolder(bldgFolder.FolderId);
                    if (mediaAssetFolder != null && _mediaService.HasChildren(mediaAssetFolder.Id))
                    {

                        //try to find existing media asset of type images or similar under the 
                        //media library for the building
                        var childrenMedia = _mediaService.GetChildren(mediaAssetFolder.Id);
                        var enumerator = childrenMedia.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            //update the assets related to the media assets in the building's folder library
                            if (enumerator.Current.Id == mediaId)
                            {
                                _assetManager.UpdateAsset(building.Id, enumerator.Current.Id);
                                mediaAssetFound = enumerator.Current;
                            }

                        }//end while
                    }
                }
                else
                {
                    //try to create the folders on db and media library
                    var result = CreateFolder(account.Name, account.Id, building.Name, building.Id);
                }
            }
            else
            {
                //if db asset found but for some reason, 
                //media was not (possibly deleted from the media library in Backoffice or in recycle bin)
                //delete the record
                if (dbMediaAsset != null && (mediaAsset == null || mediaAsset.Trashed))
                {
                    ((BuildingAssetRepository)_buildingAssetRepo).Delete(dbMediaAsset);
                }
            }

            return mediaAssetFound;

        }

        public IMedia UpdateMediaAsset(IMedia media)
        {
            _mediaService.Save(media);
            return media;
        }

        /// <summary>
        /// Method to find the existing folder from the media root folder under an account folder
        /// and tries to create the folders for the account and the building folder in the media library
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MediaFoldersResult CreateFolder(string name, int accountId = -1,
            string buildingName = "", int buildingId = -1,
            List<string> apartmentIdentifiers = null)
        {
            MediaFoldersResult result = new MediaFoldersResult();

            //assign the media account id to the result if found or created
            result.MediaAccountFolderId = CreateAccountFolders(accountId, name);

            //if building > 0, then also create the building folder for both
            //custom table and on the media library
            if (buildingId > 0)
            {
                //assign the media building id to the result if found or created
                result.MediaBuildingFolderId = CreateBuildingFolders(accountId, buildingId, buildingName, result.MediaAccountFolderId);
            }

            //try creating the apartment media folders under building folder in media library
            if (apartmentIdentifiers != null && apartmentIdentifiers.Count > 0)
            {
                //get apartments
                List<string> aptNamesFound = new List<string>();
                List<Apartment> apartmentsFound = new List<Apartment>();
                foreach (var aptIdentifier in apartmentIdentifiers)
                {
                    //var aptFound = _apartmentRepo.GetByName(aptIdentifier);
                    var aptFound = _apartmentRepo.GetByNameAndBuildingId(aptIdentifier, buildingId);
                    if (aptFound != null && !aptNamesFound.Contains(aptFound.Name))
                    {
                        aptNamesFound.Add(aptFound.Name);
                        apartmentsFound.Add(aptFound);
                    }
                }

                result.MediaApartmentFolderIds = CreateApartmentFolders(apartmentsFound);
            }

            //if found, use it, otherwise, create account folder
            //if folder created, update the accountfolder table
            //get building folder if found, add media under folder

            return result;
        }



        public void DeleteFolder()
        {
            //TODO: not handled for this version yet
        }

        /// <summary>
        /// Method async called to remove an asset from the media folder on the BO
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public bool DeleteMedia(int mediaId)
        {
            try
            {
                var mediaToRemove = _mediaService.GetById(mediaId);
                if (mediaToRemove != null && !mediaToRemove.Trashed)
                {
                    _mediaService.Delete(mediaToRemove);
                    return true;
                }
            }
            catch (System.Exception)
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// MEthod to get assets for a building from the custom 
        /// buildingAssets table
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public IAsset GetDefaultBuildingAsset(int buildingId)
        {
            IAsset assetFound = null;
            try
            {
                var buildingAssets = _buildingAssetRepo.GetAllById(buildingId);

                //if found on database
                if (buildingAssets != null && buildingAssets.Count > 0)
                {
                    foreach (var buildingAsset in buildingAssets)
                    {
                        var mediaFound = TryGetTypedMediaAsset(buildingId, buildingAsset.MediaId);

                        //first non-null result use it
                        if (mediaFound != null)
                        {
                            assetFound = buildingAsset;
                            assetFound.Url = _umbracoHelper.TypedMedia(buildingAsset.MediaId) != null ? _umbracoHelper.TypedMedia(buildingAsset.MediaId).Url : "/images/no_thumbnail.jpg";
                            break;
                        }
                    }

                    //mediaId = buildingAssets.FirstOrDefault().MediaId;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<MediaManager>(ex.Message, ex);
            }

            return assetFound;
        }

        public IList<IMedia> GetApartmentMediaAssets(int apartmentId, bool excludeFolders = true)
        {
            IList<IMedia> media = new List<IMedia>();
            var apartmentFolder = _apartmentFolderRepo.GetAll().FirstOrDefault(x => x.ApartmentId == apartmentId);
            if (apartmentFolder != null)
            {
                var mediaFolder = GetMediaFolder(apartmentFolder.FolderId);
                if (mediaFolder != null && _mediaService.HasChildren(mediaFolder.Id))
                {
                    //if found, get the assets
                    var childrenMedia = _mediaService.GetChildren(mediaFolder.Id).Where(m => !m.Trashed);
                    if (excludeFolders)
                    {
                        childrenMedia = childrenMedia.Where(m => !m.ContentType.Alias.Equals("folder", StringComparison.OrdinalIgnoreCase));
                    }

                    var enumerator = childrenMedia.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        //TODO: check if media exist/registered on apartment assets table
                        //if it exist on media library, make sure it is also registered on table
                        media.Add(enumerator.Current);
                    }
                }
            }

            return media;
        }

        public IList<IMedia> GetBuildingMediaAssets(int buildingId, bool excludeFolders = true)
        {
            IList<IMedia> media = new List<IMedia>();
            var bldgFolder = _buildingFolderRepo.GetAll().FirstOrDefault(x => x.BuildingId == buildingId);
            if (bldgFolder != null)
            {
                var mediaFolder = GetMediaFolder(bldgFolder.FolderId);
                if (mediaFolder != null && _mediaService.HasChildren(mediaFolder.Id))
                {
                    //if found, get the assets
                    var childrenMedia = _mediaService.GetChildren(mediaFolder.Id).Where(m => !m.Trashed);
                    if (excludeFolders)
                    {
                        childrenMedia = childrenMedia.Where(m => !m.ContentType.Alias.Equals("folder", StringComparison.OrdinalIgnoreCase));
                    }
                    
                    var enumerator = childrenMedia.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        //TODO: check if media exist/registered on building assets table
                        //if it exist on media library, make sure it is also registered on table
                        media.Add(enumerator.Current);
                    }
                }
            }

            return media;
        }

        /// <summary>
        /// Method to upload the media/asset files for a building or apartment
        /// Building assets are images
        /// Apartment assets are files (excel,csv,pdf, etc)
        /// </summary>
        /// <param name="files"></param>
        /// <param name="buildingId"></param>
        /// <param name="parentId"></param>
        /// <param name="apartmentId"></param>
        /// <returns></returns>
        public IList<IMedia> UploadMediaFiles(HttpFileCollectionBase files, int buildingId, int parentId, int apartmentId = -1)
        {
            IList<IMedia> media = new List<IMedia>();

            foreach (var fileKey in files)
            {
                var fileContent = files[fileKey.ToString()];
                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    fileContent.InputStream.Position = 0;
                    var binaryReader = new BinaryReader(fileContent.InputStream);
                    byte[] binData = binaryReader.ReadBytes(fileContent.ContentLength);
                    var fileName = Path.GetFileName(fileContent.FileName);

                    var mediaType = "File"; //default

                    //check if supported
                    var isImageSupported = IMAGEFORMATS_SUPPORTED.Any(x => fileContent.FileName.EndsWith(x, System.StringComparison.OrdinalIgnoreCase));
                    var isFileSupported = FILEFORMATS_SUPPORTED.Any(x => fileContent.FileName.EndsWith(x, System.StringComparison.OrdinalIgnoreCase));

                    if (isImageSupported) mediaType = "Image";

                    if (isFileSupported) mediaType = "File";

                    if (!isImageSupported && !isFileSupported) continue;

                    if (binData != null && binData.Length > 0)
                    {
                        var propertyTypeAlias = "umbracoFile";
                        var newMedia = _mediaService.CreateMedia(fileName, parentId, mediaType);
                        if (newMedia != null)
                        {
                            newMedia.SetValue(propertyTypeAlias, fileContent);
                            _mediaService.Save(newMedia);

                            //add to repository to building asset by default
                            if (apartmentId <= 0)
                            {
                                _buildingAssetRepo.Insert((BuildingAsset)AssetFactory.CreateAsset(AssetType.Building, buildingId, newMedia.Id, AssetMediaType.Image));
                            }
                            else
                            {
                                _apartmentAssetRepo.Insert((ApartmentAsset)AssetFactory.CreateAsset(AssetType.Apartment, apartmentId, newMedia.Id, AssetMediaType.File));
                            }

                            //add newly added media to list
                            media.Add(newMedia);
                        }
                    }

                }
            }
            return media;
        }



        //public void UploadFiles(HttpFileCollectionBase files, string path)
        //{
        //    foreach (var fileKey in files)
        //    {
        //        var fileContent = files[fileKey.ToString()];
        //        if (fileContent != null && fileContent.ContentLength > 0)
        //        {
        //            var inputStream = fileContent.InputStream;
        //            var fileName = Path.GetFileName(fileKey.ToString());
        //            var destination = Path.Combine(path, fileName);
        //            Upload(inputStream, fileName, destination);
        //        }
        //    }
        //}

        public void Upload(Stream stream, string filename, string destinationPath)
        {
            //TODO: upload asset via mediaService
            //var newMedia = mediaService.CreateMedia(filename, 0, "Image");
            //newMedia.SetValue("umbracoFile",)
            //mediaService.Save(newMedia);
            using (var fileStream = System.IO.File.Create(destinationPath))
            {
                stream.Position = 0; //important to reset before copying
                stream.CopyTo(fileStream);
            }
        }


        #region PRIVATE METHODS
        private void SaveMedia(IMedia mediaFolder)
        {
            _mediaService.Save(mediaFolder);
        }

        private AccountFolder SaveAccountFolder(int accountId, string name)
        {
            return _accountFolderRepo.Insert(new AccountFolder { AccountId = accountId, Name = name });
        }

        private object UpdateAccountFolder(int folderId, AccountFolder folder)
        {
            return _accountFolderRepo.Update(folderId, folder);
        }

        private object UpdateApartmentFolder(int folderId, ApartmentFolder folder)
        {
            return _apartmentFolderRepo.Update(folderId, folder);
        }

        private BuildingFolder SaveBuildingFolder(int accountId, int buildingId, string name)
        {
            return _buildingFolderRepo.Insert(new BuildingFolder { AccountId = accountId, Name = name, BuildingId = buildingId });
        }

        private object UpdateBuildingFolder(int folderId, BuildingFolder folder)
        {
            return _buildingFolderRepo.Update(folderId, folder);
        }

        private AccountFolder GetAccountFolder(int accountId)
        {
            return _accountFolderRepo.GetAll().FirstOrDefault(f => f.AccountId > 0 && f.AccountId == accountId);
        }

        private ApartmentFolder GetApartmentFolder(int aptId)
        {
            return _apartmentFolderRepo.GetAll().FirstOrDefault(f => f.ApartmentId > 0 && f.ApartmentId == aptId);
        }

        private IMedia GetMediaFolder(int mediaId)
        {
            return _mediaService.GetById(mediaId);
        }

        private BuildingFolder GetBuildingFolder(int buildingId)
        {
            return _buildingFolderRepo.GetAll().FirstOrDefault(f => f.BuildingId > 0 && f.BuildingId == buildingId);
        }

        private IMedia CreateMediaFolder(string name, int parentId, string mediaType = "Folder")
        {
            var media = _mediaService.CreateMedia(name, parentId, mediaType);
            if (media != null) SaveMedia(media);
            return media;
        }

        private bool IsMediaTrashed(IMedia media)
        {
            return media.Trashed;
        }

        private List<int> CreateApartmentFolders(List<Apartment> apartments)
        {
            try
            {
                List<int> apartmentMediaFolders = new List<int>();
                foreach (var apt in apartments)
                {
                    var apartmentFolder = GetApartmentFolder(apt.Id); //TODO: verify if Id or ROom#
                    if (apartmentFolder == null)
                    {
                        apartmentFolder = SaveApartmentFolder(apt.Id, apt.Name);
                    }

                    var mediaApartmentFolder = GetMediaFolder(apartmentFolder.FolderId);
                    //if media folder not found OR was found BUT in the recycle bin, create a new one
                    if (mediaApartmentFolder == null || (mediaApartmentFolder != null && IsMediaTrashed(mediaApartmentFolder)))
                    {
                        //get building parent folder
                        var buildingFolder = GetBuildingFolder(apt.BuildingId);

                        //create media folder for account
                        //it's -1 because it should be on the root
                        mediaApartmentFolder = CreateMediaFolder(apt.Name, buildingFolder.FolderId, "Folder");

                    }

                    //update the account folder
                    apartmentFolder.FolderId = mediaApartmentFolder.Id;
                    UpdateApartmentFolder(apartmentFolder.Id, apartmentFolder);

                    apartmentMediaFolders.Add(mediaApartmentFolder.Id);
                }

                return apartmentMediaFolders;
            }
            catch (Exception ex)
            {
                LogHelper.Error<MediaManager>(ex.Message, ex);
                return null;
            }
        }

        private ApartmentFolder SaveApartmentFolder(int apartmentId, string name)
        {
            return _apartmentFolderRepo.Insert(new ApartmentFolder {ApartmentId = apartmentId, Name = name });
        }

        private int CreateAccountFolders(int accountId, string name)
        {
            //get account folder from the custom table if it exists
            var accountFolder = GetAccountFolder(accountId);

            //if no folder found,create one on the custom table 'accountfolders'
            if (accountFolder == null)
            {
                accountFolder = SaveAccountFolder(accountId, name);
                //accountFolder.FolderId = -1; //reset to this value since the application cannot assume that the folder/media id reference still exists on the backoffice
            }

            //get the media folder for account folder object
            var mediaAccountFolder = GetMediaFolder(accountFolder.FolderId);

            //if media folder not found OR was found BUT in the recycle bin, create a new one
            if (mediaAccountFolder == null || (mediaAccountFolder != null && IsMediaTrashed(mediaAccountFolder)))
            {
                //create media folder for account
                //it's -1 because it should be on the root
                mediaAccountFolder = CreateMediaFolder(name, -1, "Folder");
            }

            //update the account folder
            accountFolder.FolderId = mediaAccountFolder.Id;
            UpdateAccountFolder(accountFolder.Id, accountFolder);

            return mediaAccountFolder.Id;

        }

        private int CreateBuildingFolders(int accountId, int buildingId, string buildingName, int mediaAccountId)
        {
            var buildingFolder = GetBuildingFolder(buildingId);
            if (buildingFolder == null)
            {
                //create if not found
                buildingFolder = SaveBuildingFolder(accountId, buildingId, buildingName);
            }

            //get the media for building folder
            var mediaBuildingFolder = GetMediaFolder(buildingFolder.FolderId);

            //if media folder not found OR was found BUT in the recycle bin, create a new one
            if (mediaBuildingFolder == null || (mediaBuildingFolder != null && IsMediaTrashed(mediaBuildingFolder)))
            {
                mediaBuildingFolder = CreateMediaFolder(buildingName, mediaAccountId, "Folder");
            }

            buildingFolder.FolderId = mediaBuildingFolder.Id;
            UpdateBuildingFolder(buildingFolder.Id, buildingFolder);

            //assign the media building id to the result if found or created
            return mediaBuildingFolder.Id;

        }
        #endregion
    }
}