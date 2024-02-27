using ShoppingML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingDAL.Repositories;

namespace ShoppingBLL
{
    public class UserBusiness
    {
        public void InsertUser(User model)
        {
            try
            {
                bool isSuccess;
                UserRepository repo = new UserRepository();
                repo.Insert(model);
            }
            catch (Exception ex)
            {
                throw new Exception("BL:UserBusiness::InsertUser::Error occured.", ex);
            }
        }
    }
}
