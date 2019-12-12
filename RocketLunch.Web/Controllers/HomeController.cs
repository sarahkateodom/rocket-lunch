using Microsoft.AspNetCore.Mvc;

namespace RocketLunch.web.controllers
{
    public class HomeController : Controller 
    {
        public IActionResult Index()
        {
            return View("~/wwwroot/app/index.html");
        }
    }
}