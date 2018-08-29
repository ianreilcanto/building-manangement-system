using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSD.SlattoFS.Repositories
{
    public class AccountRepository : PocoRepositoryBase<Account>, IPocoRepository<Account>
    {
        private List<Account> account = new List<Account>();
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
                return "SlattoSFAccounts";
            }
        }

        public IList<Account> GetAll()
        {
            return Entities;
        }

        public IList<Account> GetAllById(int id)
        {
            var account = Entities.Where(a => a.Equals(id)).ToList();
            if (account == null || account.Count == 0)
                return new List<Account>();

            return account;
        }

        public Account GetByName(string name)
        {
            var account = GetAll()
               .Where(a => a.Name == name).FirstOrDefault();

            return account;
        }
        
        public Account Insert(Account entity)
        {
            var newApp = Database.Insert(TableName, PrimaryColumn, entity);

            if (newApp == null)
                return null;

            return entity;
        }

        public bool Update(object id, Account entity)
        {
            entity.LastModifiedOn = DateTime.UtcNow;
            var updateEntityCount = Database.Update(entity, id);
            return updateEntityCount > 0;
        }

        public Account GetById(object id)
        {
            var accountId = Get(id);
            if (accountId == null)
                return null;
            return accountId;
        }
    }
}
