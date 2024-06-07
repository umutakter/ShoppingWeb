using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.DataAccessLayer;
using SampleAPI.Models;

namespace SampleAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        UsersDAL dal = new UsersDAL();

        [HttpGet]
        public List<User> GetAllUsers()
        {
            return dal.SelectAll();
        }

        [HttpGet]
        public User GetUser(int id) 
        {
            return dal.SelectById(id);
        }
    }
}
