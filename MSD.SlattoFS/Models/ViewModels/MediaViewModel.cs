using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace MSD.SlattoFS.Models.ViewModels
{
    public class MediaViewModel
    {
        public MediaViewModel()
        {
            ParentFolderId = -1;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string Url { get; set; }
        public int ParentFolderId { get; set; }

        /// <summary>
        /// factory method to construct a valid vm
        /// </summary>
        /// <param name="media"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static MediaViewModel CreateModel(IMedia media, string url)
        {
            var viewModel = new MediaViewModel();
            viewModel.SortOrder = media.SortOrder;
            viewModel.Name = media.Name;
            viewModel.Id = media.Id;
            viewModel.Url = url;
            return viewModel;
        }
    }
}