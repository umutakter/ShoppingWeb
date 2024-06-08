using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLibrary.Models;

namespace CoreLibrary.Repository
{
    public interface IBaseRepository : IDisposable
    {
        bool Update<T>(T model) where T : CoreDbModel;
        int Insert<T>(T model) where T : CoreDbModel;
        List<T> SelectAll<T>() where T : CoreDbModel;
        T SelectById<T>(int id) where T : CoreDbModel;
        void DeleteById<T>(int id) where T : CoreDbModel;

    }
}
