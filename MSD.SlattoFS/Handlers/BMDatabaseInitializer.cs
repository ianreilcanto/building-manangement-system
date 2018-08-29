using MSD.SlattoFS.Models.Pocos;
using System;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace MSD.SlattoFS.Handlers
{
    public class BMDatabaseInitializer
    {
        public static void Run(ApplicationContext appContext)
        {
            RegisterAccountDatabases(appContext);
            RegisterBuildingDatabases(appContext);
            RegisterBuildingAssetsDatabases(appContext);
            RegisterApartmentDatabases(appContext);
            RegisterAddressDatabases(appContext);
            RegisterFolderDatabases(appContext);
        }

        private static void RegisterFolderDatabases(ApplicationContext appContext)
        {
            Register<AccountFolder>(appContext, typeof(AccountFolder), "SlattoSFAccountFolders");
            Register<BuildingFolder>(appContext, typeof(BuildingFolder), "SlattoSFBuildingFolders");
            Register<ApartmentFolder>(appContext, typeof(ApartmentFolder), "SlattoSFApartmentFolders");
        }

        private static void RegisterAccountDatabases(ApplicationContext appContext)
        {
            Register<Account>(appContext, typeof(Account), "SlattoSFAccounts");
        }
        /// <summary>
        /// Method to create the usertypes on the backoffice on application startup if
        /// not yet existing on the backoffice
        /// </summary>
        private static void RunApartmentStatusesSeed()
        {
            var db = ApplicationContext.Current.DatabaseContext.Database;
            var tableName = "SlattoSFApartmentStatuses";
            var primaryColumn = "Id";
            db.Insert(tableName, primaryColumn, new ApartmentStatus { Name = "Available" });
            db.Insert(tableName, primaryColumn, new ApartmentStatus { Name = "Rented" });
            db.Insert(tableName, primaryColumn, new ApartmentStatus { Name = "Reserved" });
            db.Insert(tableName, primaryColumn, new ApartmentStatus { Name = "Unavailable" });
        }

        private static void RegisterAddressDatabases(ApplicationContext appContext)
        {
            var createTable = Register<Address>(appContext, typeof(Address), "SlattoSFAddresses");
        }

        private static void RegisterBuildingDatabases(ApplicationContext appContext)
        {
            var createTable = Register<Building>(appContext, typeof(Building), "SlattoSFBuildings");
        }

        private static void RegisterBuildingAssetsDatabases(ApplicationContext appContext)
        {
            Register<BuildingAsset>(appContext, typeof(BuildingAsset), "SlattoSFBuildingAssets");
            Register<ApartmentAsset>(appContext, typeof(ApartmentAsset), "SlattoSFApartmentAssets");
        }

        private static void RegisterApartmentDatabases(ApplicationContext appContext)
        {
            Register<SvgData>(appContext, typeof(SvgData), "SlattoSFSVGData");
            Register<Apartment>(appContext, typeof(Apartment), "SlattoSFApartments");
            var createdType = Register<ApartmentStatus>(appContext, typeof(ApartmentStatus), "SlattoSFApartmentStatuses");
            if (createdType)
            {
                RunApartmentStatusesSeed();
            }
        }

        /// <summary>
        /// Initializes, creates the database corresponding poco classes 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appContext"></param>
        /// <param name="type"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static bool Register<T>(ApplicationContext appContext, Type type, string tableName) where T : new()
        {
            var dbContext = appContext.DatabaseContext;
            var logger = LoggerResolver.Current.Logger;
            var dbSchemaUtility = new DatabaseSchemaHelper(dbContext.Database, logger, dbContext.SqlSyntax);
            if (!dbSchemaUtility.TableExist(tableName))
            {
                dbSchemaUtility.CreateTable<T>(false);
                return true;
            }
            return false;
        }
    }
}

