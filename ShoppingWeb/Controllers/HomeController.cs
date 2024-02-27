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
                Username = "Umutakter",
                Password = "123234",
                FirstName= "Umut",
                LastName="Akrer",
                Email ="akterumut@hotmail.com",
                Gender ="Male"
            };
            bl.InsertUser(user);
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