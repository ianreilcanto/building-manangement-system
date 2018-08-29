using MSD.SlattoFS.Models.Pocos;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using umbraco.BusinessLogic;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace MSD.SlattoFS.App_Plugins.Accounts.Controllers
{
    [PluginController("Accounts")]
    public class AccountUserApiController : UmbracoAuthorizedJsonController
    {
        public User GetById(int id)
        {
            return umbraco.BusinessLogic.User.GetUser(id);
            //return DatabaseContext.Database.Fetch<UmbracoUser>("SELECT * FROM umbracoUser WHERE id=" + id).FirstOrDefault();
        }

        public User PostSave(UmbracoUser user)
        {
            user.LoginName = user.Email;

            if (user.Id > 0)
            {
                User umbracoUser = new User(user.LoginName);
                umbracoUser.Name = user.Name;
                umbracoUser.LoginName = user.LoginName;
                umbracoUser.Password = HashPassword(user.Password);
                umbracoUser.Email = user.Email;

                umbracoUser.Save();
                return umbracoUser;
            }
            else
            {
                UserType userType = UserType.GetUserType(DatabaseContext.Database.Fetch<int>("SELECT id FROM umbracoUserType WHERE userTypeAlias='AccountAdministrator'").FirstOrDefault());
                umbraco.BusinessLogic.User.MakeNew(user.Name, user.LoginName, HashPassword(user.Password), userType);

                User umbracoUser = new User(user.LoginName);
                umbracoUser.Email = user.Email;
                umbracoUser.AddApplication("buildings");

                umbracoUser.Save();

                return umbracoUser;

            }
        }

        public void DeleteById(int id)
        {
            User umbracoUser = umbraco.BusinessLogic.User.GetUser(id);
            umbracoUser.ClearApplications();
            umbracoUser.delete();
            umbracoUser.Save();
        }

        public string HashPassword(string password)
        {
            HMACSHA1 hash = new HMACSHA1();
            hash.Key = Encoding.Unicode.GetBytes(password);

            string encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
            return encodedPassword;
        }
    }
}