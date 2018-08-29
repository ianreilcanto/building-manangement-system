using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Persistence;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using MSD.SlattoFS.Models.listview;
using MSD.SlattoFS.Models.Pocos;
using System;
using MSD.SlattoFS.Interface;
using MSD.SlattoFS.Services;

namespace MSD.SlattoFS.App_Plugins.Accounts.Controllers
{
    [PluginController("Accounts")]
    public class AccountApiController : UmbracoAuthorizedJsonController
    {
        private readonly IUploadManager _mediaManager;
        public AccountApiController()
        {
            _mediaManager = new MediaManager(Umbraco.UmbracoContext);
        }

        public IEnumerable<Account> GetAll()
        {
            return DatabaseContext.Database.Fetch<Account>("SELECT * FROM SlattoSFAccounts WHERE Name <> 'DefaultForAccountsFolder' ORDER BY Name");
        }

        public Account GetById(int id)
        {
            return DatabaseContext.Database.Fetch<Account>("SELECT * FROM SlattoSFAccounts WHERE Id=" + id).FirstOrDefault();
        }

        public int GetAccountsFolderId()
        {
            return DatabaseContext.Database.Fetch<int>("SELECT ImageId FROM SlattoSFAccounts WHERE Name = 'DefaultForAccountsFolder' AND Id = 1").FirstOrDefault();
        }

        public Account PostSave(Account account)
        {
            if (account.Id > 0)
            {
                DatabaseContext.Database.Update(account);
            }
            else
            {
                var currentUserId = umbraco.helper.GetCurrentUmbracoUser().Id;

                account.CreatedOn = DateTime.Now;
                account.LastModifiedOn = DateTime.Now;
                account.CreatedBy = currentUserId;
                account.UserId = currentUserId;
                DatabaseContext.Database.Save(account);

                //create the folder for the account on media
                var accountMediaFolderName = string.Concat(account.Id, "_", account.Name);
                (_mediaManager as MediaManager).CreateFolder(accountMediaFolderName, account.Id);
            }

            return account;
        }

        public int PostDeleteByBatch(int[] ids)
        {
            foreach (int id in ids)
            {
                this.DeleteById(id);
            }

            return ids.Count();
        }

        public int DeleteById(int id)
        {
            return DatabaseContext.Database.Delete<Account>(id);
        }

        public AccountsPagedResult GetPaged(int itemsPerPage, int pageNumber, string sortColumn, string sortOrder, string searchTerm)
        {
            var items = new List<Account>();
            var db = DatabaseContext.Database;

            var currentType = typeof(Account);

            var query = new Sql().Select("*").From("SlattoSFAccounts");

            if (!string.IsNullOrEmpty(searchTerm))
            {
                int c = 0;
                foreach (var property in currentType.GetProperties())
                {
                    string before = "WHERE";
                    if (c > 0)
                    {
                        before = "OR";
                    }

                    var columnAttri =
                           property.GetCustomAttributes(typeof(ColumnAttribute), false);

                    var columnName = property.Name;
                    if (columnAttri.Any())
                    {
                        columnName = ((ColumnAttribute)columnAttri.FirstOrDefault()).Name;
                    }

                    query.Append(before + " [" + columnName + "] like @0", "%" + searchTerm + "%");
                    c++;
                }
            }
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortOrder))
                query.OrderBy(sortColumn + " " + sortOrder);
            else
            {
                query.OrderBy("id asc");
            }

            var p = db.Page<Account>(pageNumber, itemsPerPage, query);
            var result = new AccountsPagedResult
            {
                TotalPages = p.TotalPages,
                TotalItems = p.TotalItems,
                ItemsPerPage = p.ItemsPerPage,
                CurrentPage = p.CurrentPage,
                Accounts = p.Items.ToList()
            };
            return result;
        }
    }
}