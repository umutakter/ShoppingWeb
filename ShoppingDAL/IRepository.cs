using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingDAL
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Insert(TEntity model);
        bool Update(TEntity model);
        bool DeletedById(int id);
        TEntity SelectedById(int id);
        IList<TEntity> SelectAll();

    }
}
