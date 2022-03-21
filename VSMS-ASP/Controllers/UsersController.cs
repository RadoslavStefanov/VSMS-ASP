using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VSMS.Core.Services;
using VSMS.Core.ViewModels;
using VSMS.Infrastructure.Data;
using VSMS.Infrastructure.Data.Common;
using VSMS.Infrastructure.Data.Models;
using VSMS_ASP.Data;

namespace VSMS_ASP.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersService usersService;
        private VSMS_ASPContext context;
        public UsersController(UsersService _usersService, VSMS_ASPContext _context)
        { 
            usersService = _usersService;
            context = _context;
        }

        public async Task<IActionResult> AdminPanel(AdminPanelViewModel model)
        {
            ViewData["Argument"] = model.arg;
            if (model.arg == "ListUsers")
            {
                //var users = context.Users.ToList();
                var users = context.Users.Select(x => new AllUsersListViewModel
                {
                    UserName = x.UserName,
                    Email = x.Email,
                    Role="Not assigned!"
                }).ToList();
                View(users);
            }
            return View(ViewData);
        }

        public void ListUsers()
        {
           Response.Redirect("/Users/AdminPanel?arg=ListUsers");
        }
    }
}
