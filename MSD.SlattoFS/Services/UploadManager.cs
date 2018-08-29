using MSD.SlattoFS.Interface;
using System.IO;
using System.Web;
using Umbraco.Core.Services;
using Umbraco.Web;
using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;

namespace MSD.SlattoFS.Services
{
    public class UploadManager : IUploadManager, IMediaService
    {
        private readonly UmbracoContext _context;

        public UploadManager(UmbracoContext context)
        {
            _context = context;
        }

        private IMediaService _mediaService;
        public IMediaService MediaService {
            get
            {
                if (_mediaService == null)
                    _mediaService = _context.Application.Services.MediaService;
                return _mediaService;
            }
        }

        public void UploadFiles(HttpFileCollectionBase files, string path)
        {
            foreach (var fileKey in files)
            {
                var fileContent = files[fileKey.ToString()];
                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    var inputStream = fileContent.InputStream;
                    var fileName = Path.GetFileName(fileKey.ToString());
                    var destination = Path.Combine(path, fileName);
                    Upload(inputStream, fileName, destination);
                }
            }
        }

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

        public void RebuildXmlStructures(params int[] contentTypeIds)
        {
            throw new NotImplementedException();
        }

        public int Count(string contentTypeAlias = null)
        {
            throw new NotImplementedException();
        }

        public int CountChildren(int parentId, string contentTypeAlias = null)
        {
            throw new NotImplementedException();
        }

        public int CountDescendants(int parentId, string contentTypeAlias = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetByIds(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public IMedia CreateMedia(string name, int parentId, string mediaTypeAlias, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public IMedia CreateMedia(string name, IMedia parent, string mediaTypeAlias, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public IMedia GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetChildren(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetPagedChildren(int id, int pageIndex, int pageSize, out int totalRecords, string orderBy = "SortOrder", Direction orderDirection = Direction.Ascending, string filter = "")
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetPagedChildren(int id, long pageIndex, int pageSize, out long totalRecords, string orderBy = "SortOrder", Direction orderDirection = Direction.Ascending, string filter = "")
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetPagedDescendants(int id, int pageIndex, int pageSize, out int totalRecords, string orderBy = "Path", Direction orderDirection = Direction.Ascending, string filter = "")
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetPagedDescendants(int id, long pageIndex, int pageSize, out long totalRecords, string orderBy = "Path", Direction orderDirection = Direction.Ascending, string filter = "")
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetDescendants(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetMediaOfMediaType(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetRootMedia()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetMediaInRecycleBin()
        {
            throw new NotImplementedException();
        }

        public void Move(IMedia media, int parentId, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public void MoveToRecycleBin(IMedia media, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public void EmptyRecycleBin()
        {
            throw new NotImplementedException();
        }

        public void DeleteMediaOfType(int mediaTypeId, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public void Delete(IMedia media, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public void Save(IMedia media, int userId = 0, bool raiseEvents = true)
        {
            throw new NotImplementedException();
        }

        public void Save(IEnumerable<IMedia> medias, int userId = 0, bool raiseEvents = true)
        {
            throw new NotImplementedException();
        }

        public IMedia GetById(Guid key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetByLevel(int level)
        {
            throw new NotImplementedException();
        }

        public IMedia GetByVersion(Guid versionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetVersions(int id)
        {
            throw new NotImplementedException();
        }

        public bool HasChildren(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteVersions(int id, DateTime versionDate, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public void DeleteVersion(int id, Guid versionId, bool deletePriorVersions, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public IMedia GetMediaByPath(string mediaPath)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetAncestors(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetAncestors(IMedia media)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMedia> GetDescendants(IMedia media)
        {
            throw new NotImplementedException();
        }

        public IMedia GetParent(int id)
        {
            throw new NotImplementedException();
        }

        public IMedia GetParent(IMedia media)
        {
            throw new NotImplementedException();
        }

        public bool Sort(IEnumerable<IMedia> items, int userId = 0, bool raiseEvents = true)
        {
            throw new NotImplementedException();
        }

        public IMedia CreateMediaWithIdentity(string name, IMedia parent, string mediaTypeAlias, int userId = 0)
        {
            throw new NotImplementedException();
        }

        public IMedia CreateMediaWithIdentity(string name, int parentId, string mediaTypeAlias, int userId = 0)
        {
            throw new NotImplementedException();
        }
    }
}