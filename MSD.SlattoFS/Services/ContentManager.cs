using Umbraco.Core.Models;
using Umbraco.Web;
using MSD.SlattoFS.Shared;

namespace MSD.SlattoFS.Services
{
    public class ContentManager
    {
        private readonly UmbracoContext _context;
        public ContentManager(UmbracoContext context)
        {
            _context = context;
        }

        public virtual UmbracoContext Context
        {
            get { return _context; }
        }

        public virtual IContent InsertContent(string name, string documentTypeAlias, int parentId = 0, int userId = 0)
        {
            var contentService = _context.Application.Services.ContentService;
            IContent newContent = null;
            if(contentService != null)
            {
                newContent = contentService.CreateContent(name, parentId, documentTypeAlias, userId);
                contentService.Save(newContent);
            }

            return newContent;
        }

        public virtual IContent SaveChangesToContent(IContent content)
        {
            var contentService = _context.Application.Services.ContentService;
            if (contentService != null)
            {
                contentService.Save(content);
            }

            return content;
        }

    }
}