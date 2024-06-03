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
    public class UserRepository
    {
        private readonly BaseRepository<User> DB = new BaseRepository<User>();

        public void InsertUser(User model)
        {
            using (var db = DB) db.Insert(model);
        }
        public void UpdateUser(User model)
        {
            using (var db = DB) db.Update(model);
        }
        public List<User> SelectAllUser()
        {
            using (var db = DB) return db.SelectAll();
            
        }
        public User SelectUserById(int id)
        {
            using (var db = DB ) return db.SelectById(id);  
        }
    }
}
