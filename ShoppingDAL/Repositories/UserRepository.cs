using CoreLibrary;
using ShoppingML.DbModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingDAL.Repositories
{
    public class UserRepository: BaseRepository<User>
    {
       public void deneme()
        {
            var a = connection;
        }
    }
}
