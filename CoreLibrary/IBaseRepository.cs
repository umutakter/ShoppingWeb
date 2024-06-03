using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public interface IBaseRepository<T> : IDisposable where T : CoreDbModel
    {
        bool Update(T model);
        bool Insert(T model);
        List<T> SelectAll();
        T SelectById(int id);
        void DeleteById(int id);

    }
}
