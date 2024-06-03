﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public interface IBaseRepository<T> : IDisposable
    {
        bool Update(T model);
        bool Insert(T model);
        List<T> SelectAll();
    }
}
