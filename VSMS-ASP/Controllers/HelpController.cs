using Microsoft.AspNetCore.Mvc;

namespace VSMS_ASP.Controllers
{
    public class HelpController : Controller
    {
        public IActionResult ForgottenLogin()
        {
            return View();
        }
    }
}
