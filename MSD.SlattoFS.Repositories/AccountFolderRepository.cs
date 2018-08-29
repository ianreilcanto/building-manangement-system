using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using System.Collections.Generic;
using System.Linq;

namespace MSD.SlattoFS.Repositories
{
    public class AccountFolderRepository : PocoRepositoryBase<AccountFolder>, IPocoRepository<AccountFolder>
    {
        protected override string PrimaryColumn
        {
            get
            {
                return "Id";
            }
        }

        protected override string TableName
        {
            get
            {
                return "SlattoSFAccountFolders";
            }
        }

        public IList<AccountFolder> GetAll()
        {
            return Entities;
        }

        public IList<AccountFolder> GetAllById(int id)
        {
            var accountFolder = Entities.Where(a => a.Id.Equals(id)).ToList();
            if (accountFolder == null || accountFolder.Count == 0)
                return new List<AccountFolder>();

            return accountFolder;
        }

        public AccountFolder GetById(object id)
        {
            var accountFolder = Get(id);
            if (accountFolder == null)
                return null;
            return accountFolder;
        }

        public AccountFolder Insert(AccountFolder entity)
        {
            var newAccountFolder = Database.Insert(TableName, PrimaryColumn, entity);
            if (newAccountFolder == null)
                return null;

            return entity;
        }

        public bool Update(object id, AccountFolder entity)
        {
            var updateEntityCount = Database.Update(entity, id);
            return updateEntityCount > 0;
        }
    }
}
