using Umbraco.Core.Models;
namespace MSD.SlattoFS.Helpers
{
    public static class PublishedContentHelper
    {
        /// <summary>
        /// Get Valid Property of an umbraco published content object via its alias
        /// </summary>
        /// <param name="publishedContent"></param>
        /// <param name="propertyAlias"></param>
        /// <returns></returns>
        public static object GetValidPropertyValue(this IPublishedContent publishedContent, string propertyAlias)
        {
            object propValue = null;
            if (IsPropertyValid(publishedContent, propertyAlias))
            {
                propValue = publishedContent.GetProperty(propertyAlias).Value;
            }

            return propValue;
        }

        /// <summary>
        /// CHeck if an object property is available or set correctly as a field
        /// on a document or published content object
        /// </summary>
        /// <param name="publishedContent"></param>
        /// <param name="propertyAlias"></param>
        /// <returns></returns>
        public static bool IsPropertyValid(this IPublishedContent publishedContent, string propertyAlias)
        {
            return publishedContent.GetProperty(propertyAlias) != null
                    && publishedContent.GetProperty(propertyAlias).Value != null
                    && !string.IsNullOrWhiteSpace(publishedContent.GetProperty(propertyAlias).Value.ToString());
        }
    }
}