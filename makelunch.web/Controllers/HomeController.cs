using Microsoft.AspNetCore.Mvc;

namespace scheduler.web.Controllers
{
    /// <summary>
    /// is the Home Controller
    /// </summary>
    public class HomeController : Controller
	{
		/// <summary>
		/// index page
		/// </summary>
		/// <returns></returns>
		public IActionResult Index()
		{
			return View("~/wwwroot/index.html");
		}
	}
}
