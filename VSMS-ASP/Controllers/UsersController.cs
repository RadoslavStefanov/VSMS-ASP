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
            if (String.IsNullOrEmpty(model.UserName))
            {
                return View(new List<ErrorViewModel>() { new ErrorViewModel("Login incorrect") }, "/Users/Error");
            }
            return View();
        }
    }
}
