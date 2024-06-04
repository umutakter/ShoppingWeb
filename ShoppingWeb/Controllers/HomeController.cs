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
                Username = "UmutAkter",
                Password = "123234",
                FirstName= "Umut",
                LastName="Akter",
                Email ="akterumut@hotmail.com",
                Gender ="Male"
            };
            int id = repository.Insert(user);
            user.FirstName = "Degistirildi";
            user.ID = id;
            var response1 = repository.Update(user);
            var response2 = repository.SelectAll();
            var response3 = repository.SelectById(id);
            repository.DeleteById(id);
            return View();
        }
    }
}