using Microsoft.AspNetCore.Mvc;

namespace VSMS_ASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {_logger = logger;}


        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            { return View(); }
            else
            {return Redirect("/Identity/Account/Login");}
        }

        public IActionResult Privacy()
        {return View();}

    }
}