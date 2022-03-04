using Microsoft.AspNetCore.Mvc;
using VSMS_ASP.Models;

namespace VSMS_ASP.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var name = model.UserName;
            var pass = model.Password;
            if (String.IsNullOrEmpty(model.UserName))
            {
                TempData["msg"] = " Email or Password is wrong !";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
