using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace VSMS_ASP.Controllers
{
    public class HelpController : Controller
    {
        private UserManager<IdentityUser> userManager;
        public HelpController(UserManager<IdentityUser> usermgr)
        { userManager = usermgr; }
        public IActionResult ForgottenLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgottenLogin(string userName)
        {
            var user = await userManager.FindByEmailAsync(userName);
            if (user == null)
            { ViewBag.MSG = "Неуспешно създадена заявка."; }
            else
            {
                
            }
            return View();
        }
    }
}
