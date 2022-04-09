using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VSMS_ASP.Areas.Restricted.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "Restricted,Guest,Admin")]
        [Area("Restricted")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
