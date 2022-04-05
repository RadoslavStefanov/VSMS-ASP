using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace VSMS_ASP.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> usermgr)
        {
            userManager = usermgr;
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = userManager.GetUserName(User);
                var roles = await userManager.GetRolesAsync(await userManager.FindByEmailAsync(userName));

                if (roles[0] == "Admin")
                {
                    var isDefault = await userManager.CheckPasswordAsync(await userManager.FindByEmailAsync(userName), "123123");
                    ViewBag.IsDefault = true; ;
                }

                if (roles.Count == 0 || roles[0] == null || roles[0] == "Restricted")
                { ViewBag.IsRestricted = true; }

                return await Task.Run(() => View());
            }
            else
            {
                return await Task.Run(() => Redirect("/Identity/Account/Login"));
            }
        }

    }
}