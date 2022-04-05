using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VSMS.Core.Services;

namespace VSMS_ASP.Controllers
{
    public class HelpController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private HelpService helpService;
        public HelpController(UserManager<IdentityUser> usermgr, HelpService _helpService)
        { userManager = usermgr; helpService = _helpService; }
        public IActionResult ForgottenLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgottenLogin(string userName)
        {
            if (userName == null)
            { ViewBag.Error = "True"; }
            else
            {
                var user = await userManager.FindByEmailAsync(userName);
                if (user == null)
                {ViewBag.Error = "True"; }
                else
                {
                    var result = helpService.CreateResetRequest(user.UserName);
                    if (result != null && result == true)
                    {ViewBag.Error = "False"; }
                    else
                    {ViewBag.Error = "True"; }
                }                
            }
            return View();
        }
    }
}
