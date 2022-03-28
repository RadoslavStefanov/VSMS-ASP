using Microsoft.AspNetCore.Authorization;
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


        public IActionResult ListUsers()
        {
            List<AllUsersListViewModel> users = new List<AllUsersListViewModel>();
            ViewData["View"] = "Users";
            users = context.Users.Select(x => new AllUsersListViewModel
            {
                UserName = x.UserName,
                Email = x.Email,
                Role =
                context.Roles
                .Where(y => y.Id == context.UserRoles
                .Where(i => i.UserId == x.Id)
                .FirstOrDefault().RoleId)
                .FirstOrDefault().Name ?? "Not Configured/Limited"
            }).ToList();
            return View(users);
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(string arg)
        {
            ViewData["UserMail"] = arg;
            return View();
        }


        
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.OldEmail);
            if (model.RoleChange != "NoChange")
            {
                var roles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, roles.ToArray());
                await userManager.AddToRoleAsync(user, model.RoleChange);
            }


            if (model.PasswordChange != null)
            {
                var hasher = new PasswordHasher<IdentityUser>();
                var hashedPass = hasher.HashPassword(null, model.PasswordChange);
                user.PasswordHash = hashedPass;
                context.SaveChanges();
            }

            if (model.EmailChange != null)
            {
                await userManager.SetEmailAsync(user, model.EmailChange);
                await userManager.SetUserNameAsync(user, model.EmailChange);
            }
            return Redirect("/Users/ListUsers");
        }

    }
}
