using Microsoft.AspNetCore.Mvc;
using VSMS.Core.Services;
using VSMS.Core.ViewModels;

namespace VSMS_ASP.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersService usersService;
        public UsersController(UsersService _usersService)
        { usersService = _usersService; }


        public IActionResult Login()
        { return View(); }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            (string tempData, bool isCorrect) = usersService.IsLoginCorrect(model);
            if (isCorrect)
            { 
                var isLoggedIn = usersService.LogIn(model);
                if (isLoggedIn!=null)
                {
                    //Go to mainPage
                }
                else
                {
                    TempData["msg"] = "Потребителят или паролата не съвпадат с базата!";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["msg"] = tempData;
                return RedirectToAction("Index", "Home");
            }
            //This will need to be delete later!
            return View();
        }

        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}
