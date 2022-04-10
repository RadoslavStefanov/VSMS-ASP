using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VSMS_ASP.Areas.Restricted.Controllers
{
    public class HomeController : Controller
    {
        [Area("Restricted")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
