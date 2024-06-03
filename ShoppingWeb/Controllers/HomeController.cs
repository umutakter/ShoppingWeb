using CoreLibrary.Logging;
using log4net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingDAL.Repositories;
using ShoppingML.DbModels;
using ShoppingWeb.Models;
using System.Diagnostics;

namespace ShoppingWeb.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILog log = Logger.GetLogger(typeof(HomeController));

        public IActionResult Index()
        {
            log.Info("Index action executed.");
            var repository = new UserRepository();
            User user = new User()
            {
                ID = 4,
                Username = "UmutAkter",
                Password = "123234",
                FirstName= "Umut",
                LastName="Akter",
                Email ="akterumut@hotmail.com",
                Gender ="Male"
            };
            repository.InsertUser(user);
            repository.UpdateUser(user);
            var response1 = repository.SelectAllUser();
            var response2 = repository.SelectUserById(5);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}