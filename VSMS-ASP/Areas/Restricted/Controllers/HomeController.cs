using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VSMS_ASP.Areas.Restricted.Models;

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

        [Authorize(Roles = "Restricted,Guest,Admin")]
        [HttpPost]
        public async Task<IActionResult> Index(PurchaseRequestViewModel model)
        {
            return await Task.Run(() => Redirect("/Restricted/"));
        }
    }
}
