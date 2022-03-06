using Microsoft.AspNetCore.Mvc;

namespace VSMS_ASP.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error404()
        {
            return View();
        }
    }
}
