using ShoppingML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingDAL.Repositories;
using CoreLibrary;

namespace ShoppingBLL
{
    public class UserBusiness
    {
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
