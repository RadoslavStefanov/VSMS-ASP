using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VSMS.Core.ViewModels;
using VSMS_ASP.Data;

namespace VSMS_ASP.Controllers
{
    public class UsersController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private VSMS_ASPContext context;
        public UsersController(VSMS_ASPContext _context, UserManager<IdentityUser> usermgr)
        {
            context = _context;
            userManager = usermgr;
        }


        public void ListUsers()
        {Response.Redirect("/AdminPanel/Show?arg=ListUsers");}

        public async Task<IActionResult> Edit(string arg)
        {
            ViewData["UserMail"] = arg;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.OldEmail);
            await userManager.RemoveFromRoleAsync(user, "Admin");
            await userManager.RemoveFromRoleAsync(user, "Employee");
            await userManager.RemoveFromRoleAsync(user, "Guest");
            await userManager.RemoveFromRoleAsync(user, "Restricted");
            var result = await userManager.AddToRoleAsync(user, model.RoleChange);
             
            if (model.PasswordChange != null)
            {
                var hasher = new PasswordHasher<IdentityUser>();
                var hashedPass = hasher.HashPassword(null, model.PasswordChange);
                user.PasswordHash = hashedPass;
                context.SaveChanges();
            }

            if (model.EmailChange != null)
            {
                user.Email = model.EmailChange;
                user.UserName = model.EmailChange;
                user.NormalizedEmail = model.EmailChange.ToUpper();
                user.NormalizedUserName = model.EmailChange.ToUpper();
                context.SaveChanges();
            }

            return Redirect("/AdminPanel/Show?arg=ListUsers");
        }

    }
}
