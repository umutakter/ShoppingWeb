using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingBLL;
using ShoppingML;
using ShoppingWeb.Models;
using System.Diagnostics;

namespace ShoppingWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var bl = new UserBusiness();
            User user = new User()
            {
                ID = 4,
                Username = "degşstirildi",
                Password = "123234",
                FirstName= "Umut",
                LastName="Akrer",
                Email ="akterumut@hotmail.com",
                Gender ="Male"
            };
            bl.InsertUser(user);
            bl.UpdateUser(user);
            var response = bl.SelectAllUser();
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