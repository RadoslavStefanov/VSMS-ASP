using Microsoft.AspNetCore.Mvc;

namespace VSMS_ASP.Controllers
{
    public class SalesController : Controller
    {
        public IActionResult CashRegister()
        {
            return View();
        }
    }
}
