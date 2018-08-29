using MSD.SlattoFS.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace MSD.SlattoFS.Handlers
{
    public class BMApplicationInitializer
    {
        public static void Run(ApplicationContext appContext)
        {
            InitializeMemberGroupTypes();
            InitializeMemberTypes();
            //InitializeUserTypes();
        }

        private static void InitializeMemberTypes()
        {
            var memberTypeService = ApplicationContext.Current.Services.MemberTypeService;
            if (memberTypeService != null)
            {
                //check if usertype exist
                var acctAdminType = memberTypeService.Get("accountAdministrator");

                //IContentTypeComposition composition = new ContentTypeComposition()
                if (acctAdminType == null)
                {

                    acctAdminType = new MemberType(-1);
                    acctAdminType.Alias=  "accountAdministrator";
                    acctAdminType.Name=  "Account Administrator";
                    memberTypeService.Save(acctAdminType);
                }

                var siteAdminType = memberTypeService.Get("siteAdministrator");
                if (siteAdminType == null)
                {
                    siteAdminType = new MemberType(-1);
                    siteAdminType.Alias = "siteAdministrator";
                    siteAdminType.Name = "Site Administrator";
                    memberTypeService.Save(siteAdminType);
                }
            }
        }

        private static void InitializeMemberGroupTypes()
        {
            var memberGroupService = ApplicationContext.Current.Services.MemberGroupService;
            if (memberGroupService != null)
            {
                //check if usertype exist
                var acctAdminType = memberGroupService.GetByName("AccountAdministratorGroup");
                if (acctAdminType == null)
                {
                    acctAdminType = new MemberGroup { Name = "AccountAdministratorGroup", Key = Guid.NewGuid() };
                    memberGroupService.Save(acctAdminType);
                }

                var siteAdminType = memberGroupService.GetByName("SiteAdministratorGroup");
                if (siteAdminType == null)
                {
                    siteAdminType = new MemberGroup { Name = "SiteAdministratorGroup", Key = Guid.NewGuid() };
                    memberGroupService.Save(siteAdminType);
                }
            }
        }

        /// <summary>
        /// //Create user types out of the userservice umbraco feature
        /// </summary>
        private static void InitializeUserTypes()
        {
            var userService = ApplicationContext.Current.Services.UserService;
            if (userService != null)
            {
                //check if usertype exist
                var acctAdminType = userService.GetUserTypeByAlias("accountAdministrator");
                if (acctAdminType == null)
                {
                    acctAdminType = new AccountAdminUserType { Alias = "accountAdministrator", Name = "Account Administrator" };
                    userService.SaveUserType(acctAdminType);
                }

                var siteAdminType = userService.GetUserTypeByAlias("siteAdministrator");
                if (siteAdminType == null)
                {
                    siteAdminType = new AccountAdminUserType { Alias = "siteAdministrator", Name = "Site Administrator" };
                    userService.SaveUserType(siteAdminType);
                }

            }
        }
    }
}