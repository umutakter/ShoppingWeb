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
            try
            {
                using (var db = DB)
                {
                    db.Insert(model);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("ShoppingDAL:UserRepository::InsertUser::Error occured.", ex);
            }
        }
        public void UpdateUser(User model)
        {
            try
            {
                using (var db = DB)
                {
                    db.Update(model);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ShoppingDAL:UserRepository::UpdateUser::Error occured.", ex);
            }
        }
        public List<User> SelectAllUser()
        {
            try
            {
                using (var db = DB)
                {
                    return db.SelectAll();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ShoppingDAL:UserRepository::SelectAllUser::Error occured.", ex);
            }
        }
    }
}
