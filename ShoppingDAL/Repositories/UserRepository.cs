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
                DB.Insert(model);
            }
            catch (Exception ex)
            {
                throw new Exception("BL:UserBusiness::InsertUser::Error occured.", ex);
            }
        }
        public void UpdateUser(User model)
        {
            try
            {
                DB.Update(model);
            }
            catch (Exception ex)
            {
                throw new Exception("BL:UserBusiness::UpdateUser::Error occured.", ex);
            }
        }
        public List<User> SelectAllUser()
        {
            try
            {
                return DB.SelectAll<User>();
            }
            catch (Exception ex)
            {
                throw new Exception("BL:UserBusiness::UpdateUser::Error occured.", ex);
            }
        }
    }
}
