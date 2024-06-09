using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.DataAccessLayer;
using SampleAPI.Models;
using ShoppingML.Attributes;

namespace SampleAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [CoreAuthorization]
    public class UsersController : ControllerBase
    {
        UsersDAL dal = new UsersDAL();

        [HttpGet]
        public List<User> GetAllUsers()
        {
            return dal.SelectAll<User>();
        }

        [HttpGet]
        public User GetUser(int id) 
        {
            return dal.SelectById<User>(id);
        }
        [HttpPost]
        public int InsertUser(User model)
        {
            return dal.Insert(model);
        }
    }
}
