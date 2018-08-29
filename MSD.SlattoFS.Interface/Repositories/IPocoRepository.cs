using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSD.SlattoFS.Interfaces.Repositories
{
    public interface IPocoRepository<TModel> 
    where TModel : class
    {
        IList<TModel> GetAll();
        IList<TModel> GetAllById(int id);
        TModel GetById(object id);
        TModel Insert(TModel entity);
        bool Update(object id, TModel entity);

    }
}
