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
                //usersService.Login(model); 
            }
            else
            {
                TempData["msg"] = tempData;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
