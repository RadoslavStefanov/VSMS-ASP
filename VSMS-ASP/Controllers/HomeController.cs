using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace VSMS_ASP.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<IdentityUser> userManager;

        public HomeController(ILogger<HomeController> logger,
               UserManager<IdentityUser> usermgr)
        {userManager = usermgr;}


        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = userManager.GetUserName(User);
                var roles = await userManager.GetRolesAsync(await userManager.FindByEmailAsync(userName));

                if (roles.Count == 0)
                {
                    ViewBag.IsRestricted = true;
                    return await Task.Run(() => Redirect("/Restricted/"));
                }

                if (roles[0] == "Guest")
                {
                    ViewBag.IsRestricted = true;
                    return await Task.Run(() => Redirect("/Restricted/"));
                }

                if (roles.Count == 0 || roles[0] == null || roles[0] == "Restricted")
                { ViewBag.IsRestricted = true; }

                return await Task.Run(() => View());
            }
            
            return await Task.Run(() => Redirect("/Identity/Account/Login"));
        }

    }
}