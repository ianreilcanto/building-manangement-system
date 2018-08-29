using MSD.SlattoFS.Repositories.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace MSD.SlattoFS.Repositories
{
    public abstract class PocoRepositoryBase<T> where T : class
    {
        private Database _database;
        protected abstract string PrimaryColumn { get; }
        protected abstract string TableName { get; }

        protected PocoRepositoryBase()
        {
            _database = ApplicationContext.Current.DatabaseContext.Database;
        }

        protected Database Database
        {
            get
            {
                if(_database == null)
                {
                    _database = ApplicationContext.Current.DatabaseContext.Database;
                }
                return _database;
            }
        }

        private IList<T> _entities;

        protected IList<T> Entities
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TableName))
                {
                    return new List<T>();
                }

                var query = string.Format("SELECT * FROM {0}", TableName);
                if(_entities == null)
                {
                    _entities = _database.Fetch<T>(query);
                }

                return _entities;
            }
        }

        protected T Get(object id)
        {
            var query = string.Format("SELECT * from {0} WHERE {1} = {2}", 
                TableName, 
                PrimaryColumn, 
                id.ToString());
            return _database.FirstOrDefault<T>(query);
        }

        protected bool Delete(object id)
        {
            try
            {
                _database.Delete(Get(id));
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public PageResult<T> GetPaged(int itemsPerPage, int pageNumber, 
            string sortColumn, string sortOrder, string searchTerm)
        {
            var items = new List<T>();
            var currentType = typeof(T);

            var query = new Sql().Select("*").From(TableName);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                int counter = 0;

                foreach (var property in currentType.GetAllProperties())
                {
                    string before = "WHERE";
                    if ( counter > 0 )
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

                    query.Append(before + "[" + columnName + "] like @0", "%" + searchTerm + "%");
                    counter++;
                }
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortOrder))
                query.OrderBy(sortColumn + " " + sortOrder);
            else
            {
                query.OrderBy("id asc");
            }

            var p = _database.Page<T>(pageNumber, itemsPerPage, query);
            var result = new PageResult<T>
            {
                TotalPages = p.TotalPages,
                TotalItems = p.TotalItems,
                ItemsPerPage = p.ItemsPerPage,
                CurrentPage = p.CurrentPage,
                Items = p.Items.ToList()

            };

            return result;
        }
    }
}
